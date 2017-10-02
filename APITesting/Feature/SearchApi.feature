Feature: APITests
	In order to search 
	As a API Consumer
	I want the API to send me proper failure response codes
	so that I can handle them and show appropriate error pages to user

@searchNormal
Scenario Outline: Search API called with parameters
	Given I call Search API with parameters <store>, '<lang>', '<currency>', '<query>', <offset>, <limit>
	When I get the response back from API 
	Then Search API returns '<status>'
Examples: 
| testCase        | store | lang | currency | query | offset | limit | status                |
| MissingCurrency | 1     | en   |          | red   | 0      | 2     | ERROR_MISSINGCURRENCY |
| MissingLanguage | 2     |      | GBP      | green | 0      | 10    | ERROR_MISSINGLANGUAGE |
| MissingQuery    | 1     | en   | EUR      |       | 0      | 2     | ERROR_MISSINGQUERY    |
| MissingStore    | 0     | en   | GBP      | red   | 0      | 2     | ERROR_MISSINGSTORE   |


@searchFail
Scenario Outline: Search API called with different request types
	Given I make a '<requestType>' call to Search API without parameters
	When I get the response back from API
	Then API returns <failureresponsecode>
Examples:
| testCase        | requestType     | failureresponsecode |
| TypeGet         | GET             | 400                 |
| TypePost        | POST            | 403                 |
| TypeGetWrongURL | GETWITHWRONGURL | 404                 |
| TypeHead        | HEAD            | 405                 |
| TypePut         | PUT             | 501                 |

# Similarly add more cases if API is returning any other 4xx Client Error or 5xx Server Error Code.
              

@additionalSearchTest
Scenario Outline: Search API called with parameters and match the count as per the parameters
	Given I call Search API with parameters <store>, '<lang>', '<currency>', '<query>', <offset>, <limit>
	When I get the response back from API 
	Then Search API returns product <count>
Examples: 
| testCase          | store | lang | currency | query | offset | limit | count |
| WithoutLimitParam | 1     | en   | GBP      | red   | 0      | 0     | 250   |
| WithLimitParam    | 1     | en   | GBP      | red   | 0      | 10    | 10    |



