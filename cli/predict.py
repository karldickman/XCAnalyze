#!/usr/bin/env python2.6

"""Predict the outcome of a season of Cross-Country.  Searches the database
for the PRs of each runner in a season and scores those PRs like a race."""

from datetime import date
import hytek
from hytek import ResultsDumper, ScoreDumper
from itertools import count
from miscellaneous import main_function
from operator import attrgetter
from sqlobject.sqlbuilder import AND, IN, INNERJOINOn
from xcanalyze.models import Affiliations, Conferences, Distances, \
        DidNotCompeteError, Runners, Schools
from xcanalyze.scoring import AggregatedResults

from common import connect, ConnectionError, GenderedOptionParser, \
        race_display_options, RaceDumper, SeasonsBestDumper

def parse_arguments(arguments):
    "Parse command line arguments.  Returns the tuple (options, (gender,))."
    option_parser = race_display_options(GenderedOptionParser())
    option_parser.add_option("-c", "--conference", help="Include schools from "
                             "only the specified conference.")
    option_parser.add_option("-d", "--dist-limit", help="Minimum race "
                             "distance.", type="int")
    option_parser.add_option("-f", "--filter", help="Include or exclude the "
                             "specified schools.")
    option_parser.add_option("-p", "--previous-years", default=0, type="int")
    option_parser.add_option("-y", "--year", default=date.today().year,
                             type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    index = count()
    try:
        connect(options.server)
        gender = arguments[next(index)]
    except IndexError:
        option_parser.error("")
    except ConnectionError, error:
        option_parser.error(error)
    if options.conference is not None:
        options.conference = Conferences.get(options.conference)
        options.filter = list(Schools.selectBy(conference=options.conference))
    else:
        options.filter = option_parser.parse_schools(options.filter)
    if options.dist_limit is None:
        if gender == "M":
            options.dist_limit = Distances.get(1).mens_distance
        else:
            options.dist_limit = Distances.get(1).womens_distance
    return options, (gender,)

@main_function(parse_arguments)
def main(options, (gender,)):
    results = assemble_results(gender, options.dist_limit, options.filter,
                               options.year, options.previous_years)
    results.score()
    if options.exclude_scoreless:
        results.purge_scoreless_runners()
    if options.show_races:
        for row in RaceDumper(results.races, gender):
            print row
        print
        for row in SeasonsBestDumper(results.results,
                                            options.dist_limit):
            print row
    else:
        for row in ResultsDumper(results.results, options.dist_limit):
            print row
    print
    for row in ScoreDumper(results.scores):
        print row

def assemble_results(gender, dist_limit, school_filter, year, previous_years):
    """Assembles results based on the criteria given."""
    best_times = []
    query = AND(Runners.q.gender == gender, Affiliations.q.year == year,
                IN(Affiliations.q.school, school_filter))
    join = INNERJOINOn(Runners, Affiliations,
                       Runners.q.id == Affiliations.q.runner)
    for affiliation in Affiliations.select(query, join=join):
        runner = affiliation.runner
        seasons_bests = []
        for i in xrange(previous_years + 1):
            try:
                seasons_bests.append(runner.seasons_best(dist_limit, year - i))
            except DidNotCompeteError:
                continue
        try:
            best_times.append(min(seasons_bests, key=attrgetter("time")))
        except ValueError:
            pass
    best_times.sort()
    races = list(set(result.race for result in best_times))
    for i, race in enumerate(sorted(races, key=attrgetter("date"))):
        race.num = i
    final_results = [Finisher(result.runner.name, result.time,
                              result.runner.year,
                              result.runner.school(year).name, result.race.num)
                     for result in best_times]
    predicted_results = AggregatedResults(final_results, races)
    return predicted_results

class Finisher(hytek.Finisher):
    """Minor modification of hytek.Finisher."""
    def __init__(self, name, time, year, school, race_num):
        super(Finisher, self).__init__(name, time, year, school)
        self.race_num = race_num

if __name__ == "__main__":
    main()
