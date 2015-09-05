#!/usr/bin/python

"""List all runners in the database."""

from formatting import Table
from miscellaneous import main_function
from xcanalyze.models import Runners

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options,
    arguments)."""
    option_parser = OptionParser()
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    runners = irows(Runners.select())
    for runner in Table(runners, column_seperator=" | "):
        print runner

def irows(runners):
    """Construct the rows of the table."""
    for runner in runners:
        yield [runner.id, runner.name, runner.gender,
               "; ".join(ischools(runner))]

def ischools(runner):
    """Construct the school cell of the table."""
    for school, years in runner.schools().iteritems():
        years = (str(year) for year in years)
        yield school.name + " (" + ", ".join(years) + ")"

if __name__ == "__main__":
    main()
