#!/usr/bin/python

"""Replace runners and schools with their ID numbers from the database."""

from datetime import date
from re import IGNORECASE
from re import compile as Regex
from miscellaneous import main_function
from sqlobject.sqlbuilder import INNERJOINOn
from sys import stderr, stdin
from yaml import dump, load
from xcanalyze.models import Affiliations, Runners, Schools

from common import connect, ConnectionError, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options, arguments)."""
    option_parser = OptionParser()
    option_parser.add_option("-y", "--year", default=date.today().year)
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    """Read YAML race description from stdin and print transformed race to
    stdout."""
    try:
        print transform(stdin.read(), options.year)
    except KeyboardInterrupt:
        return 1
    except KeyError, error:
        print >> stderr, error
        return 1

def transform(string, year):
    """Where possible, replace all runner's with their database ID numbers.
    May raise a key error."""
    result_list = load(string)
    schools = [(school, Regex("|".join(school.names())))
               for school in Schools.select()]
    for (school, pattern) in schools:
        for item in result_list:
            if pattern.match(item["school"]):
                item["school_id"] = school.id
                del item["school"]
    join = INNERJOINOn(Runners, Affiliations,
                       Runners.q.id == Affiliations.q.runner)
    for runner in Runners.select(Affiliations.q.year == year, join=join):
        for name in runner.given_names:
            last_first = r"%s,\s*%s" % (runner.surname, name)
            first_last = r"%s\s*%s" % (name, runner.surname)
            pattern = Regex(last_first + "|" + first_last, IGNORECASE)
            for item in result_list:
                if pattern.match(item["name"]):
                    item["runner_id"] = runner.id
                    del item["name"]
                    del item["school_id"]
    return dump(result_list)

if __name__ == "__main__":
    main()
