from datetime import date as Date
from sqlite3 import connect

def main():
    connection = connect("example.xca")
    cursor = connection.cursor()
    # Create the states
    print get_insert_command(cursor, "states", "state_code", "name")
    # Create the cities
    print get_insert_command(cursor, "cities", "city_id", "name", "state_code")
    # Create the conferenes
    print get_insert_command(cursor, "conferences", "conference_id", "name", "acronym")
    # Get the runners
    print get_insert_command(cursor, "runners", "runner_id", "surname", "given_name", "gender")
    # Get the enrollment years
    print get_insert_command(cursor, "college_enrollment_years", "runner_id", "enrollment_year")
    # Get results for unknown races
    print get_insert_command(cursor, "results_unknown_race", "runner_id", "season", "time")
    # Get runner nicknames
    print get_insert_command(cursor, "runner_nicknames", "runner_id", "nickname")
    # Get teams
    print get_insert_command(cursor, "teams", "team_id", "name")
    # Get team nicknames
    print get_insert_command(cursor, "team_nicknames", "team_id", "nickname")
    # Get unaffiliated teams
    print get_insert_command(cursor, "unaffiliated_teams", "team_id")
    # Get conference affiliations
    print get_insert_command(cursor, "conference_affiliations", "team_id", "conference_id")
    # Get affiliations
    print get_insert_command(cursor, "affiliations", "runner_id", "team_id", "season")
    # Get venues
    print get_insert_command(cursor, "venues", "venue_id", "name", "city_id")
    # Get meets
    print get_insert_command(cursor, "meets", "meet_id", "name")
    # Get meet hosts
    print get_insert_command(cursor, "meet_hosts", "meet_id", "team_id")
    # Get meet instances
    print get_insert_command(cursor, "meet_instances", "meet_id", "date", "venue_id")
    # Get meet instance hosts
    print get_insert_command(cursor, "meet_instance_hosts", "meet_id", "date", "team_id")
    # Get races
    print get_insert_command(cursor, "races", "race_id", "distance", "gender", "meet_id", "date")
    # Get results
    print get_insert_command(cursor, "results", "runner_id", "race_id", "time")
    # DNFS
    print get_insert_command(cursor, "did_not_finish", "race_id", "runner_id")

def get_insert_command(cursor, table, *column_list):
    columns = ", ".join(column_list)
    select = "SELECT %s FROM %s" % (columns, table)
    cursor.execute(select)
    return insert_command(table, column_list, cursor)

def insert_command(table, column_list, row_list):
    columns = ", ".join(column_list)
    rows = ",\n    ".join(map(format_row, row_list))
    if len(rows) > 0:
        command = "INSERT INTO %s\n    (%s)\n    VALUES\n    %s;"
        return command % (table, columns, rows)
    return "-- No values found for %s" % table

def format_row(value_list):
    return "(%s)" % ", ".join(map(format_value, value_list))

def format_value(value):
    try:
        int_value = int(value)
        return str(value)
    except:
        pass
    try:
        float_value = float(value)
        return str(value)
    except:
        pass
    try:
        date_value = Date(value)
        return "\"%d-%d-%d\"" % (date_value.year, date_value.month, date_value.day)
    except:
        pass
    return "\"%s\"" % value

if __name__ == "__main__":
    main()
