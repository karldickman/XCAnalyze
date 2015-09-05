#!/usr/bin/python

"""List all the venues in the database."""

import formatting
from miscellaneous import main_function
from xcanalyze.models import Venues

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command-line arguments."""
    option_parser = OptionParser()
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    venues = ivenues(Venues.select())
    for row in formatting.Table(venues, column_seperator=" | "):
        print row

def ivenues(venues):
    """Generate rows for the output table."""
    for venue in venues:
        try:
            elevation = int(venue.elevation)
        except TypeError:
            elevation = None
        yield [venue.name, venue.city, venue.state, elevation]

if __name__ == "__main__":
    main()
