"""A library of useful functions to help the scripts in this directory.  This
includes shorthand methods of querying the database."""

from hytek import DefaultTable, ResultsDumper
from optparse import OptionParser as BuiltinOptionParser
from os import environ as environment
from sqlobject import connectionForURI, sqlhub, SQLObjectNotFound
from sqlobject.dberrors import OperationalError
from sqlobject.main import SQLObjectNotFound
from xcanalyze.models import Distances, Schools, TooManyObjectsError

CNX_ENV_VAR = "XCA_CNX"

def connect(uri):
    """Create a connection to the database at the specified URI.  Raises
    ConnectionError if any problems are encountered."""
    try:
        sqlhub.processConnection = connectionForURI(uri)
    except KeyError:
        raise ConnectionError(uri + " is not a valid database URI.")
    except AssertionError, error:
        requested_driver = uri.split(":")[0]
        raise ConnectionError("No database driver exists for " +
                              requested_driver)
    #Check that communication with the given database is possible
    try:
        Distances.get(1)
    except OperationalError, error:
        raise ConnectionError(error)
    except SQLObjectNotFound:
        pass

def default_server():
    """Get the default server defined in the environment variable XCA_CNX."""
    try:
        return environment[CNX_ENV_VAR]
    except KeyError:
        return None

def gender_callback(option, option_string, value, parser):
    """Process gender information."""
    try:
        setattr(parser.values, option.dest, parse_gender(value, option_string))
    except InvalidGenderError, error:
        parser.error(error)

def parse_gender(value, option_string="gender"):
    """Process gender information."""
    if value not in ("M", "F", "W"):
        message = "Expected one of \"M\" or \"F\" for %s, not %s." % \
                (option_string, value)
        raise InvalidGenderError(message)
    return "F" if value == "W" else value

#Classes

class OptionParser(BuiltinOptionParser):
    """The common option parser to be shared by all scripts in this
    directory."""

    NO_SERVER_MESSAGE = "A database URI must be defined in either the -s, " + \
    "--server flag or the " + CNX_ENV_VAR + " environment variable."

    def __init__(self, *args, **kwargs):
        BuiltinOptionParser.__init__(self, *args, **kwargs)
        self.add_option("-s", "--server", default=default_server(),
                        help="URI for the SQL server")

    @staticmethod
    def parse_schools(string):
        """Parse a comma-separated list of numbers and integers into a list of
        School instances."""
        schools = set(school for school in Schools.select())
        try:
            string = string.lstrip()
        except AttributeError:
            return list(schools)
        if string[:3] == "NOT":
            avoid = set()
            for label in string[3:].split(","):
                label = label.strip()
                try:
                    avoid.update(Schools.get(label))
                except TooManyObjectsError:
                    pass
                except SQLObjectNotFound:
                    pass
            return list(schools - avoid)
        else:
            include = []
            for label in string.split(","):
                label = label.strip()
                try:
                    include.append(Schools.get(label))
                except TooManyObjectsError:
                    pass
                except SQLObjectNotFound:
                    pass
            return include

class GenderedOptionParser(OptionParser):
    """Force the first argument to the program to be a gender, which must be
    'M' or 'F'."""
    def __init__(self, *args, **kwargs):
        OptionParser.__init__(self, *args, **kwargs)
        self.set_usage("%prog")

    def parse_args(self, arguments=None, values=None):
        options, arguments = OptionParser.parse_args(self, arguments, values)
        if len(arguments) == 0:
            self.error("Required argument GENDER missing.")
        try:
            gender = parse_gender(arguments[0])
        except InvalidGenderError, error:
            self.error(error)
        return options, [gender] + arguments[1:]

    def set_usage(self, usage):
        if hasattr(usage, "replace"):
            usage = usage.replace("%prog", "%prog GENDER")
        OptionParser.set_usage(self, usage)

def race_display_options(option_parser):
    """Add options to the option parser that manipulate the display of race
    information."""
    option_parser.add_option("-r", "--show-races", action="store_true",
                             default=False, help="Print information about the "
                             "race in the header.")
    option_parser.add_option("-x", "--exclude-scoreless", action="store_true",
                             default=False, help="Do not show scoreless "
                             "runners in the results.")
    return option_parser

class RaceDumper(DefaultTable):
    """Dump race results in a HyTek-style format."""
    def __init__(self, races, gender):
        headings = ["", "Race name", "Date", "Location", "", "", "Distance"]
        pads = [str.rjust, None, None, None, None, None, None]
        for i, race in enumerate(races):
            race.num = i
            rows = [[race.num, race.meet.name[:20], race.date,
                     race.venue.name[:20], race.venue.city, race.venue.state,
                     int(race.distance(gender))] for race in races]
        super(RaceDumper, self).__init__(rows, headings=headings, pads=pads)

class SeasonsBestDumper(ResultsDumper):
    """For results aggregated from multiple races, include a list of races
    before the results and a numerical key for each time to indicate in which
    race it was run."""
    def __init__(self, results, distance):
        super(SeasonsBestDumper, self).__init__(results, distance)
        self.headings.append("Race")
        self.pads.append(str.rjust)
        for result, row in zip(results, self.rows):
            row.append(result.race_num)
        self.columns = ResultsDumper.columns(self.rows, self.headings,
                                             self.pads)
        for column in self.columns:
            column.pad_items()

class ConnectionError(Exception): pass
class InvalidGenderError(Exception): pass
