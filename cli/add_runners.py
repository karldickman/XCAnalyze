#!/usr/bin/python
"""Add a list of runners into the database."""

from miscellaneous import main_function
from sqlobject import SQLObjectNotFound
from sys import stderr, stdin
from xcanalyze.models import Runners, Schools
from yaml import load
from yaml.scanner import ScannerError

from common import connect, ConnectionError, gender_callback, OptionParser

def parse_arguments(arguments):
    """Parse command line arguments.  Returns the tuple (options,
    arguments)."""
    option_parser = OptionParser()
    option_parser.add_option("-g", "--gender", action="callback",
                             callback=gender_callback, type="str")
    option_parser.add_option("--school")
    options, arguments = option_parser.parse_args(arguments[1:])
    try:
        connect(options.server)
    except ConnectionError, error:
        option_parser.error(error)
    if options.school is not None:
        options.school = Schools.get(options.school)
    return options, arguments

@main_function(parse_arguments)
def main(options, arguments):
    """Read a YAML description of the runners from stdin, and save it to the
    database."""
    try:
        runners = load(stdin.read())
        verify(runners, options)
    except KeyboardInterrupt:
        return 1
    except ScannerError, error:
        print >> stderr, error
        return 1
    except (SQLObjectNotFound, VerificationError), error:
        print >> stderr, error
        return 1
    for runner in runners:
        Runners.create(runner["given_name"], runner["surname"],
                       runner["school"], runner["gender"], runner["year"],
                       runner["nicknames"], runner["competition_year"])

def verify(runners, options):
    """Check that all runners have the fields required by Runners.create().
    Raises a VerificationError if any problems are encountered."""
    for runner in runners:
        #Leaving out the names is not allowed
        for field in ("given_name", "surname"):
            if field not in runner:
                raise VerificationError("Required field \"%s\" left blank." %
                                        field)
        #School must be defined
        if "school" not in runner:
            if options.school is None:
                raise VerificationError("Required field \"school\" left blank.")
            runner["school"] = options.school
        else:
            runner["school"] = Schools.get(runner["school"])
        #Gender must be defined
        if "gender" not in runner:
            if options.gender is None:
                raise VerificationError("Required field \"gender\" left blank.")
            runner["gender"] = options.gender
        for field in ("year", "nicknames", "competition_year"):
            if field not in runner:
                runner[field] = None

class VerificationError(Exception): pass

if __name__ == "__main__":
    main()
