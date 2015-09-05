#!/usr/bin/python

"""Reads the database and prints out a list of the schools."""

from miscellaneous import main_function
from xcanalyze.models import Schools

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, arguments)."""
    option_parser = OptionParser()
    option_parser.add_option("-p", "--pretty", action="store_true",
                             default=False, help="Print database ID and "
                             "official name.  If not set, the program prints "
                             "out a list of all nicknames as well.")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    if options.pretty:
        for school in Schools.select():
            print school.id, school.full_name
    else:
        for school in Schools.names():
            print school

if __name__ == "__main__":
    main()
