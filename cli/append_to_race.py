#!/usr/bin/python

"""Add results to a race already in the database."""

from datetime import date
from miscellaneous import main_function
from optparse import OptionParser
from sys import stderr, stdin
from xcanalyze.models import Races
from yaml import load
from yaml.scanner import ScannerError

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, race_id)."""
    option_parser = OptionParser()
    option_parser.set_usage("%prog [options] RACE_ID")
    option_parser.add_option("-y", "--year", default=date.today().year,
                             help="Year in which the race occurred.",
                             type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
        race = Races.get(int(arguments[0]))
    except ConnectionError, error:
        option_parser.error(error)
    except ValueError:
        race = Races.get(arguments[0], options.year)
    except IndexError:
        option_parser.error("Required positional argument missing.")
    return options, race

@main_function(parse_arguments)
def main(options, race):
    """Read YAML race results from stdin and save them to the specified
    race."""
    try:
        results = load(stdin.read())
    except KeyboardInterrupt:
        return 1
    except ScannerError, error:
        print >> stderr, error
        return 1
    Races.add_results(race, results)

if __name__ == "__main__":
    main()
