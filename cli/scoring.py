#!/usr/bin/env python2.6

from hytek import ITeam, RaceTime
from operator import attrgetter, itemgetter

class NO_POINTS(object): pass

class RaceResults(object):
    """The results of a race."""

    scores = property(lambda self: self._scores)

    def __init__(self, results):
        self.results = results

    def __repr__(self):
        return "RaceResults(%s)" % (self.results)

    def purge_scoreless_runners(self):
        """Remove all runners from the results if they did not score."""
        self.score()
        self.results = [result for result in self.results if result.points not
                        in (NO_POINTS, None)]

    def score(self):
        """
        Scores a race based on its finish list.
        """
        self.results.sort(key=attrgetter("time"))
        teams = {}
        #Make a list of teams
        for result in self.results:
            if result.team is None:
                result.points = NO_POINTS
            else:
                try:
                    teams[result.team].append(result)
                except KeyError:
                    teams[result.team] = [result]
        #Tag runners on teams with fewer than five as scoreless
        #Tag runner 8 and beyond on each team as scoreless
        for team in teams.itervalues():
            if len(team) < 5:
                for runner in team:
                    runner.points = NO_POINTS
            elif len(teams) > 7:
                for runner in team[7:]:
                    runner.points = NO_POINTS
        #Tag each runner with their places
        for i, result in enumerate(self.results):
            presult = self.results[i-1]
            if result.time != presult.time:
                result.place = i + 1
            else:
                result.place = presult.place
        #Tag each runner with their points
        results_with_points = [result for result in self.results if
                               result.points is not NO_POINTS]
        for i, result in enumerate(results_with_points):
            presult = results_with_points[i-1]
            if result.time != presult.time:
                result.points = i + 1
            else:
                result.points = presult.points
        #Compute the team scores
        with_scores = []
        without_scores = []
        for team_name, team in teams.iteritems():
            if team[0].points is not NO_POINTS:
                score = sum(runner.points for runner in team[:5])
                top_five = sum((runner.time for runner in team[:5]),
                               RaceTime(0))
                top_five /= 5
                if len(team) > 5:
                    top_seven = sum((runner.time for runner in team[:7]),
                                    RaceTime(0))
                    top_seven /= min(len(team), 7)
                else:
                    top_seven = None
                summary = Team(team_name, team, top_five, top_seven, score)
                with_scores.append(summary)
            else:
                top_five = sum((runner.time for runner in team), RaceTime(0))
                top_five /= len(team)
                top_seven = None
                summary = Team(team_name, team, top_five, top_seven)
                without_scores.append(summary)
        with_scores.sort(key=attrgetter("score"))
        without_scores.sort(key=attrgetter("top_five"))
        for i, team in enumerate(with_scores):
            team.place = i + 1
        self._scores = with_scores
        self._scores += without_scores
        #Convert NO_POINTS to None
        for result in self.results:
            if result.points is NO_POINTS:
                result.points = None
        for team in without_scores:
            team.points = None

class AggregatedResults(RaceResults):
    """The results of multiple races."""

    def __init__(self, results, races):
        super(AggregatedResults, self).__init__(results)
        self.races = sorted(races, key=attrgetter("date"))

    def __repr__(self):
        return "AggregatedResults(%s, %s)" % (self.results, self.races)

class Team(ITeam):
    """A simple implementation of the ITeam interface."""
    __slots__ = ["place", "name", "finishers", "top_five", "top_seven", "score"]

    def __init__(self, name, finishers, top_five, top_seven=None, score=None,
                 place=None):
        self.name = name
        self.finishers = finishers
        self.top_five = top_five
        self.top_seven = top_seven
        self.place = place
        self.score = score
