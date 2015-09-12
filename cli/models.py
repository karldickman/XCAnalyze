"""Table models and useful data structures."""

from datetime import date
from extensions.collections import OrderedDict
from operator import attrgetter, eq, ge
from sqlobject import BoolCol, DateCol, EnumCol, FloatCol, ForeignKey, IntCol
from sqlobject import RelatedJoin, sqlhub, SQLObject, SQLObjectNotFound
from sqlobject import StringCol
from sqlobject import connectionForURI as connection_for_URI
from sqlobject.sqlbuilder import AND, IN, INNERJOINOn, OR
from sys import stderr

from scoring import RaceTime

#Table wrappers
def searchable_by_name(get_by_name=None):
    """Add a get_by_name function to the class, and modify the get function so
    that it attempts to call get_by_name if it cannot resolve the requested id
    to an integer."""
    if get_by_name is None:
        def get_by_name(cls, name, connection=None):
            """Find the object with the given name."""
            found = cls.selectBy(name=name, connection=connection)
            if found.count() == 1:
                return found[0]
            if found.count() == 0:
                raise SQLObjectNotFound("Could not find \"%s\"." % name)
            raise TooManyObjectsError
    get_by_name = classmethod(get_by_name)
    def decorator(to_decorate):
        @classmethod
        def get(cls, *args, **kwargs):
        #def get(cls, id_, connection=None, selectResults=None):
            try:
                args = [int(args[0])] + list(args[1:])
                return super(to_decorate, cls).get(*args, **kwargs)
            except ValueError:
                return cls.get_by_name(*args, **kwargs)
        to_decorate.get = get
        to_decorate.get_by_name = get_by_name
        return to_decorate
    return decorator

class Affiliations(SQLObject):
    """Who ran for which school in which year."""
    runner = ForeignKey("Runners", cascade=False)
    school = ForeignKey("Schools", cascade=False)
    year = IntCol()

def get_conference_by_name(cls, name, connection=None):
    query = OR(cls.q.name == name, cls.q.abbreviation == name)
    found = cls.select(query, connection=connection)
    if found.count() == 1:
        return found[0]
    if found.count() == 0:
        raise SQLObjectNotFound("Could not find \"%s\"." % name)
    raise TooManyObjectsError

@searchable_by_name(get_conference_by_name)
class Conferences(SQLObject):
    name = StringCol()
    abbreviation = StringCol()

class Distances(SQLObject):
    """The distances at which the men and women compete."""
    mens_distance = IntCol()
    womens_distance = IntCol()

@searchable_by_name()
class Meets(SQLObject):
    """A meet is a recurring race."""
    name = StringCol()

def get_race_by_name(cls, name, year=date.today().year, connection=None):
    meet = Meets.get_by_name(name)
    query = AND(cls.q.meet == meet,
                cls.q.date >= date(year, 1, 1),
                cls.q.date <= date(year, 12, 31))
    found = cls.select(query, connection=connection)
    if found.count() == 1:
        return found[0]
    if found.count() == 0:
        raise SQLObjectNotFound("Could not find \"%s\"." % name)
    raise TooManyObjectsError("More than one race \"%s\" found." % name)

@searchable_by_name(get_race_by_name)
class Races(SQLObject):
    """A race is one instance of a meet."""
    meet = ForeignKey("Meets", cascade=False)
    date = DateCol()
    venue = ForeignKey("Venues", cascade=False)
    mens_distance = IntCol()
    womens_distance = IntCol()
    comments = StringCol()

    def distance(self, gender):
        """Get the distance run based on gender."""
        gender = gender.lower()
        men = ["m", "men", "male", "man", "mens"]
        women = ["w", "women", "f", "female", "woman", "womens"]
        if gender in men:
            return self.mens_distance
        if gender in women:
            return self.womens_distance
        raise ValueError("'%s' is not a gender." % gender)

    @property
    def location(self):
        """Get the location of the race."""
        return ", ".join([str(self.venue), str(self.city), str(self.state)])

    #Helper methods

    def add_results(self, results):
        """Add the given results to the database."""
        for result in results:
            Results(runner=result["runner_id"], race=self, time=result["time"])

    @classmethod
    def create(cls, name, date, venue, mens_distance, womens_distance, comments,
               results):
        """Save a race and its results to the database."""
        try:
            meet = Meets.get_by_name(name)
        except SQLObjectNotFound:
            print >> stderr, "No meet %s found; creating new one."
            meet = Meets(name=name)
        race = cls(meet=meet, date=date, venue=venue,
                   mens_distance=mens_distance, womens_distance=womens_distance,
                   comments=comments)
        race.add_results(results)

class Results(SQLObject):
    """Who ran which time at which race."""
    runner = ForeignKey("Runners", cascade=False)
    race = ForeignKey("Races", cascade=False)
    time = FloatCol()
    #points = None
    #place = None

    def _get_time(self):
        return RaceTime(self._SO_get_time())

    #Helper methods

    def scaled_time(self, scale_to):
        """Linearly scale the time to another race distance."""
        gender = self.runner.gender
        return RaceTime(self._SO_get_time() / self.race.distance(gender) *
                                scale_to)

def get_runner_by_name(cls, surname, given_name=None, year=None,
                       connection=None):
    """Searches for a runner with the given name and graduation year..  If
    none are found, raises SQLObjectNotFound.  If more than one is found,
    raises TooManyObjectsError."""
    query = cls.q.surname == surname
    if given_name is not None:
        query = AND(query, OR(cls.q.given_name == given_name,
                              IN(cls.q.nicknames, given_name)))
    if year is not None:
        query = AND(query, cls.q.year == year)
    found = cls.select(query, connection=connection)
    if found.count() == 1:
        return found[0]
    if found.count() == 0:
        raise SQLObjectNotFound("Could not find \"%s, %s (%s)\"." %
                                (surname, given_name, year))
    raise TooManyObjectsError("More than one runner \"%s, %s (%s)\" found." %
                              (surname, given_name, year))

@searchable_by_name(get_runner_by_name)
class Runners(SQLObject):
    surname = StringCol()
    given_name = StringCol()
    nicknames = StringCol()
    gender = EnumCol(enumValues=["M", "F"])
    year = IntCol()

    def _get_nicknames(self):
        nicknames = self._SO_get_nicknames()
        if nicknames is None:
            return []
        return [nickname.strip() for nickname in nicknames.split(",")]

    @property
    def given_names(self):
        """The runner's given name and all nicknames."""
        return [self.given_name] + self.nicknames

    @property
    def name(self):
        """The runner's full name."""
        return self.given_name + " " + self.surname

    #Helper methods

    @classmethod
    def create(cls, given_name, surname, school, gender, year=None,
               nicknames=None, competition_year=None):
        """Create a new runner and their affiliations."""
        if competition_year is None:
            competiton_year = date.today().year
        runner = cls(given_name=given_name, surname=surname, school=school,
                     gender=gender, nicknames=nicknames, year=year)
        Affiliations(runner=runner, school=school, year=competition_year)

    def performances(self, distance, year=None, strict=False):
        """All the runenrs performances at the given distance.  If strict is
        true, only races that are exactly the given distance are considered.
        Otherwise, all longer races are scaled down to the given distance."""
        join = INNERJOINOn(Races, Results, Results.q.race == Races.q.id)
        compare = [ge, eq][strict]
        distance_query = {"M": compare(Races.q.mens_distance, distance),
                          "F": compare(Races.q.womens_distance, distance)}
        distance_query = distance_query[self.gender]
        query = AND(distance_query, Results.q.runner == self)
        if year is not None:
            query = AND(Races.q.date >= date(year, 1, 1), Races.q.date <=
                        date(year, 12, 31), query)
        return Results.select(query, join=join)

    def personal_record(self, distance, strict=False):
        """The runner's personal record at the given distance.  If strict is
        true, will only return those results at exactly the given distance."""
        return min(self.performances(), key=attrgetter("time"))

    def school(self, year=date.today().year):
        """Indicates which school the runner was affiliated with in the given
        year.  Raises a NotAffiliatedError if there is no such school."""
        try:
            return Affiliations.selectBy(runner=self, year=year)[0].school
        except IndexError:
            raise NotAffiliatedError

    def schools(self):
        """The schools the runner has been affiliated with."""
        schools = OrderedDict()
        affiliations = Affiliations.select(Affiliations.q.runner == self,
                                           orderBy="year")
        for affiliation in affiliations:
            try:
                schools[affiliation.school].append(affiliation.year)
            except KeyError:
                schools[affiliation.school] = [affiliation.year]
        return schools

    def schools_by_year(self):
        """The schools the runner is affiliated with by year."""
        years = OrderedDict()
        affiliations = Affiliations.select(Affiliations.q.runner == self,
                                           orderBy="year")
        for affiliation in affiliations:
            years[affiliation.year] = affiliation.school
        return years

    def seasons_best(self, distance, season, strict=False):
        """Finds a runners season's best in the specified season.  Raises a
        DidNotCompeteError if they did not compete in that season.  If strict
        is true, considers only those races at the exact distance given."""
        found = self.performances(distance, season, strict)
        try:
            return min(found, key=attrgetter("time"))
        except ValueError:
            raise DidNotCompeteError

@searchable_by_name()
class Schools(SQLObject):
    name = StringCol()
    nicknames = StringCol()
    type = StringCol()
    name_order = BoolCol(default=True)
    conference = ForeignKey("Conferences", cascade=False)

    def _get_nicknames(self):
        nicknames = self._SO_get_nicknames()
        if nicknames is None:
            return []
        return [nickname.lstrip() for nickname in nicknames.split(",")]

    @property
    def full_name(self):
        """The full name of the school, such as "University of Puget Sound."""
        if self.name_order:
            if self.type:
                return self.name + " " + self.type
            return self.name
        return self.type + " of " + self.name

    @classmethod
    def names(cls):
        """Generate all the possible names of the schools in the database."""
        for school in cls.select():
            for school_name in [school.name] + school.nicknames:
                yield school_name

    #Helper methods

    def team(self, year=date.today().year, gender=None):
        """Search the database for the team fielded in the specified year by the
        specified school."""
        key = lambda runner: (runner.surname, runner.given_name)
        join = INNERJOINOn(Affiliations, Runners, Affiliations.q.runner ==
                           Runners.q.id)
        query = AND(Affiliations.q.school == self, Affiliations.q.year == year)
        if gender is not None:
            query = AND(query, Runners.q.gender == gender)
        affiliations = Affiliations.select(query, join=join)
        runners = (affiliation.runner for affiliation in affiliations)
        return sorted(runners, key=key)

def get_venue_by_name(cls, name, city=None, state=None):
    """Find the venue with the given name.  City or state may be specified
    for the sake of disambiguation.  Raises a TooManyObjectsError if more
    than one venue is found."""
    found = cls.selectBy(name=name)
    if city is not None:
        found = [venue for venue in found if venue.city == city]
    if state is not None:
        found = [venue for venue in found if venue.state == state]
    if found.count() == 1:
        return found[0]
    if found.count() == 0:
        raise SQLObjectNotFound("No venue named %s could be found." % name)
    raise TooManyObjectsError("More than one venue %s found." % name)

@searchable_by_name(get_venue_by_name)
class Venues(SQLObject):
    name = StringCol()
    city = StringCol()
    state = StringCol()
    elevation = IntCol()

tables = [Affiliations, Conferences, Distances, Meets, Races, Results, Runners, Schools,
          Venues]

#Exceptions

class DidNotCompeteError(Exception): pass
class NotAffiliatedError(Exception): pass
class TooManyObjectsError(Exception): pass

if __name__ == "__main__":
    uri = "mysql://xcanalyze@localhost/xca_database"
    sqlhub.processConnection = connection_for_URI(uri)
