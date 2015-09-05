#!/usr/bin/python

"""Show the results of a race."""

from datetime import date
from hytek import Finisher, ResultsDumper, ScoreDumper
from miscellaneous import main_function
from sqlobject.main import SQLObjectNotFound
from sqlobject.sqlbuilder import IN, INNERJOINOn
from xcanalyze.models import Races, Results, NotAffiliatedError
from xcanalyze.scoring import AggregatedResults

from common import connect, ConnectionError, GenderedOptionParser, \
        InvalidGenderError, parse_gender, race_display_options, RaceDumper, \
        SeasonsBestDumper

def parse_arguments(arguments):
    """Parse command-line arguments.  Returns the tuple (options, (gender,
    races))."""
    option_parser = race_display_options(GenderedOptionParser())
    option_parser.set_usage("%prog RACE_ID [RACE_IDS...] [options]")
    option_parser.add_option("-d", "--distance",
                             help="Preferred race distance.", type="int")
    option_parser.add_option("-y", "--year", default=date.today().year,
                             type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
        gender, race_ids = parse_gender(arguments[0]), arguments[1:]
        if len(race_ids) == 0:
            option_parser.error("Please specify at least one race.")
        races = [None] * len(race_ids)
        for i, race_id in enumerate(race_ids):
            try:
                races[i] = Races.get(int(race_id))
            except ValueError:
                races[i] = Races.get(race_id, options.year)
    except ConnectionError, error:
        option_parser.error(error)
    except InvalidGenderError, error:
        option_parser.error(error)
    except SQLObjectNotFound, error:
        option_parser.error(error)
    return options, (gender, races)

@main_function(parse_arguments)
def main(options, (gender, races)):
    """Aggregate the results of the given races."""
    results = aggregate_races(gender, races)
    results.score()
    if options.exclude_scoreless:
        results.purge_scoreless_runners()
    if options.show_races:
        for row in RaceDumper(results.races):
            print row
        print
        for row in SeasonsBestDumper(results.results, options.distance):
            print row
    else:
        for row in ResultsDumper(results.results, options.distance):
            print row
    print
    for row in ScoreDumper(results.scores):
        print row

def aggregate_races(gender, races, distance=None):
    """Aggregate the results of the given races, converting them to the given
    distance."""
    if distance is None:
        distance = races[0].distance
    year = races[0].date.year
    join = INNERJOINOn(Results, Races, Results.q.race == Races.q.id)
    select_by = IN(Results.q.race, races)
    runners = []
    results = []
    distance_query = {"M": Races.q.mens_distance, "F": Races.q.womens_distance}
    query = Results.select(select_by, join=join, orderBy=Results.q.time /
                           distance_query[gender])
    for result in query:
        runner = result.runner
        if runner.gender == gender:
            if runner not in runners:
                runners.append(runner)
                try:
                    school = runner.school(year).name
                except NotAffiliatedError:
                    school = None
                results.append(Finisher(runner.name, result.time, runner.year,
                                        school))
    results = AggregatedResults(results, races)
    return results

if __name__ == "__main__":
    main()
