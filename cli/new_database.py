#!/usr/bin/env python2.6
"""Create a new XCAnalyze database."""

from getpass import getpass
from itertools import count
from miscellaneous import main_function
from optparse import OptionParser
from sqlobject import connectionForURI as connection_for_URI, sqlhub

from models import Distances, tables

DB_PREFIX = "xca_"
USERNAME = "xcanalyze"

def parse_arguments(arguments):
    """Parse command line arguments.  Returns a tuple (options, (system,
    host, name, (mens_distance, womens_distance))."""
    usage = "%prog [options] SYSTEM HOST NAME MENS_DISTANCE WOMENS_DISTANCE"
    option_parser = OptionParser(usage)
    option_parser.add_option("-u", "--username", default=USERNAME)
    options, arguments = option_parser.parse_args(arguments[1:])
    index = count(0)
    try:
        system = arguments[index.next()]
        host = arguments[index.next()]
        name = arguments[index.next()]
        mens_distance = int(arguments[index.next()])
        womens_distance = int(arguments[index.next()])
    except IndexError:
        option_parser.error("Please provide the correct number of positional "
                            "arguments.")
    except ValueError, error:
        option_parser.error(error)
    return options, (system, host, name, (mens_distance, womens_distance))

@main_function(parse_arguments)
def main(options, arguments):
    system, host, name, distances = arguments
    root_password = getpass()
    create_database(system, host, root_password, name, options.username,
                    distances)

def create_database(system, host, root_password, name, username, distances):
    """Create and initialize the database."""
    cnx_uri = uri(system, host, None, "root", root_password)
    root_cnx = connection_for_URI(cnx_uri)
    root_cnx.query("CREATE DATABASE %s%s" % (DB_PREFIX, name))
    root_cnx.query("GRANT ALL ON %s%s.* TO %s@%s IDENTIFIED BY ''" %
                   (DB_PREFIX, name, USERNAME, host))
    root_cnx.query("USE %s%s" % (DB_PREFIX, name))
    root_cnx.close()
    connection = connection_for_URI(uri(system, host, name))
    sqlhub.processConnection = connection
    for table in tables:
        table.createTable()
    Distances(mens_distance=distances[0], womens_distance=distances[1])

def uri(system, host, dbname, username=USERNAME, password=None):
    """Create a URI for a database with the given host and name."""
    dbname = "" if dbname is None else dbname
    dbname = DB_PREFIX + dbname if len(dbname) > 0 else ""
    if host == "sqlite":
        return host + "/" + dbname
    else:
        password = ":" + password if password is not None else ""
        return system + "://" + username + password + "@" + host + "/" + dbname

if __name__ == "__main__":
    main()
