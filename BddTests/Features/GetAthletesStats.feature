Feature: GetAthletesStats

As a consumer of the API
I want to get the stats of an athlete

Scenario: Gets an authorised athlete's stats successfully
	Given the athlete is authorised
	When I send a GET request to "/api/athlete/{athleteId}/stats"
	Then the response status code should be 200
	And the response body should be a JSON object containing the athlete's stats

Scenario: Get an unauthorised athlete's stats
	Given the athlete is not authorised
	When I send a GET request to "/api/athlete/{athleteId}/stats"
	Then the response status code should be 401

