#!/usr/bin/python

"""Shows a summary of a runner's performance over their career."""

from formatting import Table
from miscellaneous import main_function
from sqlobject import SQLObjectNotFound
from sys import stderr
from xcanalyze.models import Results, Runners, TooManyObjectsError

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, runners).
    May raise a ConnectionError"""
    option_parser = OptionParser("%prog RUNNER_ID [RUNNER_IDS...]")
    option_parser.add_option("-t", "--sort-times", action="store_true",
                             default=False)
    options, runner_ids = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    runners = []
    for runner_id in runner_ids:
        try:
            runners.append(parse_runner(runner_id))
        except (TooManyObjectsError, SQLObjectNotFound), error:
            print >> stderr, error
    return options, runners

@main_function(parse_arguments)
def main(options, runners):
    school_format = lambda (school, years): \
            school.name + " (" + ", ".join(map(str, years)) + ")"
    for runner in runners:
        print runner.name, "(%d):" % runner.year,
        print ", ".join(map(school_format, runner.schools().iteritems()))
        results = iresults(runner, options.sort_times)
        for result in Table(results, column_seperator=" | "):
            print result
    if len(runners) == 0:
        return 1

def iresults(runner, sort_times=False):
    """Generate the rows of the output table."""
    results = Results.selectBy(runner=runner)
    def key(result):
        if sort_times:
            return result.race.distance(runner.gender), result.time
        return result.race.date
    results = sorted(results, key=key)
    for result in results:
        distances = {"M": result.race.mens_distance, "F":
                     result.race.womens_distance}[runner.gender]
        race = result.race
        yield [race.date, race.meet.name, result.time, distances]

def parse_runner(runner):
    """Parse a runner from the provided string."""
    try:
        return Runners.get(int(runner))
    except ValueError:
        if "," in runner:
            surname, given_name = runner.split(",")
        elif " " in runner:
            given_name, surname = runner.split(" ")
        else:
            return Runners.get(runner)
        return Runners.get(surname.strip(), given_name.strip())

if __name__ == "__main__":
    main()
