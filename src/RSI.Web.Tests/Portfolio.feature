Feature: Portfolio
	In order to verify that the portfolio is correctly displayed
	As an investor
	I want to see all my funds

@mytag
Scenario: All ETFs are shown
	Given I navigate to the portfolio page
	Then there are 6 etfs in the table
