#!/usr/bin/python

"""Format a results sheet produced by the Hy-tek Meet Manager into the easier
to parse YAML format."""

from hytek import load, LoadError
from miscellaneous import main_function
from sys import stderr, stdin
from yaml import dump

@main_function()
def main(options, arguments):
    """Read a file on stdin and dump a yaml file to stdout."""
    try:
        results = load(stdin.read())
    except KeyboardInterrupt:
        return 1
    except LoadError, error:
        print >> stderr, error
        return 1
    results = [{"name": runner.name, "school": runner.team, "time":
                runner.time.seconds + runner.time.microseconds / 1000000.0} for
               runner in results]
    print dump(results)

if __name__ == "__main__":
    main()
