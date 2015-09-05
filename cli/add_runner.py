#!/usr/bin/python

"""Save a new runner to the database."""

from datetime import date
from itertools import count
from miscellaneous import main_function
from optparse import OptionParser
from sqlobject import SQLObjectNotFound
from xcanalyze.models import Runners, Schools

from common import connect, ConnectionError, InvalidGenderError, \
        OptionParser, parse_gender

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, (given_name,
    surname, school, gender)).  May raise a ConnectionError."""
    option_parser = OptionParser()
    option_parser.set_usage("%prog [options] GIVEN_NAME SURNAME SCHOOL GENDER")
    option_parser.add_option("--competition-year", default=date.today().year,
                             type="int")
    option_parser.add_option("-n", "--nicknames")
    option_parser.add_option("-y", "--year", help="Graduation year", type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    index = count(0)
    try:
        connect(options.server)
        given_name = arguments[index.next()]
        surname = arguments[index.next()]
        school_id = Schools.get(arguments[index.next()])
        gender = parse_gender(arguments[index.next()])
    except ConnectionError, error:
        option_parser.error(error)
    except IndexError:
        option_parser.error("Exactly 4 positional arguments must be provided.")
    except InvalidGenderError, error:
        option_parser.error(error)
    except SQLObjectNotFound, error:
        option_parser.error(error)
    return options, (given_name, surname, school_id, gender)

@main_function(parse_arguments)
def main(options, (given_name, surname, school, gender)):
    """Add a new runner to the database from the command-line arguments."""
    Runners.create(given_name, surname, school, gender, options.year,
                   options.nicknames, options.competition_year)

if __name__ == "__main__":
    main()
