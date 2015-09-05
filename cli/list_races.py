#!/usr/bin/python

"""Retrieves the races available from the database and shows their names or
other attributes as specified. """

from formatting import Table
from miscellaneous import main_function
from xcanalyze.models import Races

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse the command-line arguments.  Returns the tuple (options,
    arguments)."""
    option_parser = OptionParser()
    option_parser.add_option("--show-venues", action="store_true",
                             default=False, help="Show the venue where each "
                             "race was held.")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    print options.show_venues
    def key(race):
        try:
            return (race.date, race.meet.name)
        except AttributeError:
            return (race.date,)
    races = sorted(Races.select(), key=key)
    rows = irows(races, options.show_venues)
    for row in Table(rows, column_seperator=" | "):
        print row

def irows(races, show_venues):
    """Generate all the rows of the output table."""
    for race in races:
        try:
            mens_distance = int(race.mens_distance)
        except TypeError:
            mens_distance = None
        try:
            womens_distance = int(race.womens_distance)
        except TypeError:
            womens_distance = None
        name = race.meet.name if race.meet is not None else None
        row = [race.id, name, race.date, mens_distance,
                     womens_distance]
        if show_venues:
            if race.venue is not None:
                row += [race.venue.name, race.venue.city, race.venue.state]
            else:
                row += [None] * 3
        yield row

if __name__ == "__main__":
    main()
