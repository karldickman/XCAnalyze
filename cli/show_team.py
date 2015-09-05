#!/usr/bin/env python2.6

"""List the members on a team in a particular year."""

from datetime import date
from formatting import Table
from itertools import count
from miscellaneous import main_function
from operator import attrgetter
from sqlobject import SQLObjectNotFound
from xcanalyze.models import Schools, TooManyObjectsError

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, (school,)).
    May raise a ConnectionError."""
    option_parser = OptionParser("%prog SCHOOL")
    option_parser.add_option("-g", "--sort-by-gender", action="store_true",
                             default=False)
    option_parser.add_option("-y", "--year", default=date.today().year,
                             type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    index = count()
    try:
        school = Schools.get(arguments[next(index)])
    except (TooManyObjectsError, SQLObjectNotFound), error:
        option_parser.error(error)
    except IndexError:
        option_parser.error("Required argument not provided.")
    return options, (school,)

@main_function(parse_arguments)
def main(options, (school,)):
    runners = irunners(school, options.year, options.sort_by_gender)
    pads = [str.rjust, None, None, None]
    runners = Table(runners, column_seperator=" | ", pads=pads)
    for runner in runners:
        print runner

def irunners(school, year, sort_by_gender=False):
    """Generate the rows of the output table."""
    keys = [lambda runner: (runner.surname, runner.given_name),
            attrgetter("gender")]
    runners = sorted((runner for runner in school.team(year)),
                     key=keys[sort_by_gender])
    for runner in runners:
        yield runner.id, runner.name, runner.gender, runner.year

if __name__ == "__main__":
    main()
