Feature: GetAthlete

As an API consumer
I want to be able to get an athlete

Scenario: Get an authorised athlete's information successfully
	Given the athlete is authorised
	When I send a GET request to "/api/athlete"
	Then the response status code should be 200
	And the response should contain the athlete's information

Scenario: Get an unauthorised athlete's information
	Given the athlete is not authorised
	When I send a GET request to "/api/athlete"
	Then the response status code should be 401