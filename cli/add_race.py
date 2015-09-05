#!/usr/bin/python

"""Log the results of a race to the database."""

import datetime
from itertools import count
from miscellaneous import main_function
from sqlobject import SQLObjectNotFound
from sys import stderr, stdin
from xcanalyze.models import Distances, Races, Venues
from yaml import load
from yaml.scanner import ScannerError

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, (name,
    date, venue, city, state))."""
    option_parser = OptionParser()
    option_parser.set_usage("%prog [options] RACE_NAME DATE VENUE")
    option_parser.add_option("-c", "--comments", help="Comments on the race, "
                             "such as conditions, intensity of competition, "
                             "etc.")
    option_parser.add_option("-e", "--elevation", type="int")
    option_parser.add_option("-m", "--mens-distance", help="The length of "
                             "the men's race.", type="int")
    option_parser.add_option("-w", "--womens-distance", help="The length of "
                             "the women's race.", type="int")
    options, arguments = option_parser.parse_args(arguments[1:])
    index = count()
    try:
        connect(options.server)
        race_name = arguments[next(index)]
        date = Date.from_string(arguments[next(index)])
        venue = arguments[next(index)]
        venue = Venues.get(venue)
    except ConnectionError, error:
        option_parser.error(error)
    except IndexError:
        option_parser.error("Positional arguments for the race's name, date "
                            "and location should be specified.")
    except SQLObjectNotFound, error:
        try:
            venue = int(venue)
        except ValueError:
            try:
                name, city, state = venue.split(", ")
            except ValueError, error:
                option_parser.error("Could not extract a venue from \"" +
                                    venue + "\": " + error)
            venue = Venues(name=name, city=city, state=state, elevation=None)
        else:
            option_parser.error(error)
    except ValueError, error:
        option_parser.error(error)
    if options.mens_distance is None:
        options.mens_distance = Distances.get(1).mens_distance
    if options.womens_distance is None:
        options.womens_distance = Distances.get(1).womens_distance
    return options, (race_name, date, venue)

@main_function(parse_arguments)
def main(options, (name, date, venue)):
    """Reads race results from a YAML file taken from stdin and saves them to
    the database based on the command-line arguments."""
    try:
        results = load(stdin.read())
    except KeyboardInterrupt:
        return 1
    except ScannerError, error:
        print >> stderr, error
        return 1
    Races.create(name, date, venue, options.mens_distance,
                 options.womens_distance, options.comments, results)

class Date(datetime.date):
    """Clone of datetime.date that can be instantiated by parsing a string of
    the form /YYYY-MM-DD/."""

    @classmethod
    def from_string(cls, string):
        """Construct a new datetime.date object from a string matching the
        pattern \d{4}[-/]\d{1,2}[-/]\d{1,2}."""
        try:
            if "-" in string:
                year, month, day = string.split("-")
            elif "/" in string:
                year, month, day = string.split("/")
            else:
                raise ValueError("%s does not match YYYY-MM-DD" % string)
            year = int(year)
            month = int(month)
            day = int(day)
        except ValueError, error:
            raise ValueError(error)
        try:
            return datetime.date(year, month, day)
        except ValueError:
            raise TypeError("%d-%d-%d is not a valid date." %
                            (year, month, day))

if __name__ == "__main__":
    main()
