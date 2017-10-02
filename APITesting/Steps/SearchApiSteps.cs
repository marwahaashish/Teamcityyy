namespace APITesting
{
    using System.Net;
    using System;
    using NUnit.Framework;
    using RestSharp;
    using TechTalk.SpecFlow;
    using ScenarioContextEnum;
    using System.Linq;
    using System.Collections.Generic;    
    using System.Configuration;    
    using Mapping;

    [Binding]
    public class SearchApiSteps
    {
        private readonly SharedContext _context;
        // Context Injection
        public SearchApiSteps(SharedContext context)
        {
            _context = context;
        }
        #region Given
        /// <summary>
        /// Step to add the parameters in HTTP request if present.
        /// </summary>
        /// <param name="store"></param>
        /// <param name="lang"></param>
        /// <param name="currency"></param>
        /// <param name="query"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        [Given(@"I call Search API with parameters (.*), '(.*)', '(.*)', '(.*)', (.*), (.*)")]
        public void GivenICallSearchAPIWithParametersAnd(int store, string lang, string currency, string query, int offset, int limit)
        {
            _context.Request = new RestRequest(Method.GET);
            // Method to add the parameters in the request.
            BuildURLWithParameter(store, lang, currency, query, offset, limit);
        }

        /// <summary>
        /// Step to test various failure codes by changing the method type.
        /// </summary>
        /// <param name="requestType"></param>
        [Given(@"I make a '(.*)' call to Search API without parameters")]
        public void GivenIMakeACallToSearchAPIWithoutParameters(string requestType)
        {
            switch (requestType)
            {
                case "GET":
                    _context.Request = new RestRequest(Method.GET);
                    break;
                case "POST":
                    _context.Request = new RestRequest(Method.POST);
                    break;
                case "HEAD":
                    _context.Request = new RestRequest(Method.HEAD);
                    break;
                case "PUT":
                    _context.Request = new RestRequest(Method.PUT);
                    break;
                case "GETWITHWRONGURL":
                    var url = ConfigurationManager.AppSettings["WRONGURL"];
                    // Changing the value for baseUrl to wrong URL to test 404 failure.
                    _context.Client.BaseUrl = new Uri(url);
                    _context.Request = new RestRequest(Method.GET);
                    break;
                default:
                    Assert.Fail("Please provide the valid request type");
                    break;
            }
        }

        #endregion

        #region Then

        /// <summary>
        /// This step will cover :
        /// if only one parameter is missing from request 
        /// </summary>
        /// <param name="status"></param>
        [Then(@"Search API returns '(.*)'")]
        public void ThenSearchAPIReturns(string status)
        {

            // Converted to list as we can have more than one parameter missing. This test case covers only one
            // missing parameter.
            IList<ErrorResponse> response = _context.Response.Data.ToObject<IList<ErrorResponse>>();

            switch (status)
            {
                case "ERROR_MISSINGCURRENCY":
                    Assert.That(_context.Response.StatusCode == HttpStatusCode.BadRequest,
                           string.Format("Expected {0} :, Actual {1} :",HttpStatusCode.BadRequest, _context.Response.StatusCode));
                    Assert.That(response.Select(e => e.errorCode == ErrorCodeEnum.ERROR_MISSINGCURRENCY.ToString())
                          .FirstOrDefault(),"No missing currency");
                    break;
                case "ERROR_MISSINGLANGUAGE":
                    Assert.That(_context.Response.StatusCode == HttpStatusCode.BadRequest,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.BadRequest, _context.Response.StatusCode));
                    Assert.That(response.Select(e => e.errorCode == ErrorCodeEnum.ERROR_MISSINGLANGUAGE.ToString())
                          .FirstOrDefault(), "No missing language");
                    break;
                case "ERROR_MISSINGQUERY":
                    Assert.That(_context.Response.StatusCode == HttpStatusCode.BadRequest,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.BadRequest, _context.Response.StatusCode));
                    Assert.That(response.Select(e => e.errorCode == ErrorCodeEnum.ERROR_MISSINGQUERY.ToString())
                          .FirstOrDefault(), "No missing query");
                    break;
                case "ERROR_MISSINGSTORE":
                    Assert.That(_context.Response.StatusCode == HttpStatusCode.BadRequest,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.BadRequest, _context.Response.StatusCode));
                    Assert.That(response.Select(e => e.errorCode == ErrorCodeEnum.ERROR_MISSINGSTORE.ToString())
                          .FirstOrDefault(), "No missing store");
                    break;
                default:
                    Assert.Fail("Error response not handled");
                    break;

            }

        }

        /// <summary>
        /// Currently this step is checking for 400,403,404,405 & 501 failure codes.
        /// But this can be easily extended by adding more cases if we need to check for 
        /// any other failure code.
        /// </summary>
        /// <param name="failureResponseCode"></param>
        [Then(@"API returns (.*)")]
        public void ThenAPIReturns(int failureResponseCode)
        {
            var response = _context.Response;
            switch (failureResponseCode)
            {
                case 400:
                    Assert.That(response.StatusCode == HttpStatusCode.BadRequest,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.BadRequest, _context.Response.StatusCode));
                    break;
                case 403:
                    Assert.That(response.StatusCode == HttpStatusCode.Forbidden,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.Forbidden, _context.Response.StatusCode));
                    break;
                case 404:
                    Assert.That(response.StatusCode == HttpStatusCode.NotFound,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.NotFound, _context.Response.StatusCode));
                    break;
                case 405:
                    Assert.That(response.StatusCode == HttpStatusCode.MethodNotAllowed,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.MethodNotAllowed, _context.Response.StatusCode));
                    break;
                case 501:
                    Assert.That(response.StatusCode == HttpStatusCode.NotImplemented,
                           string.Format("Expected {0} :, Actual {1} :", HttpStatusCode.NotImplemented, _context.Response.StatusCode));
                    break;
                // Can add more cases if API is returning any other 4xx Client Error
                //5xx Server Error codes
                default: Assert.Fail("Failure code passed is not handled");
                    break; 
            }
        }

        /// <summary>
        /// Step to match the product count returned by search API.
        /// </summary>
        /// <param name="productCount"></param>
        [Then(@"Search API returns product (.*)")]
        public void ThenSearchAPIReturnsProduct(int productCount)
        {
            // Below we are checking for number of products returned in
            // API response, Similartly we can do the assert and verification 
            // on any other attribute returned in response.

            var products = _context.Response.Data.products;

            switch (productCount)
            {
                case 250:
                    Assert.That(products.Count == productCount);
                    break;
                case 10:
                    Assert.That(products.Count == productCount);
                    break;
                default:
                    Assert.Fail("Case not handled please validate the test data passed");
                    break;
            }
        }

        #endregion

        #region Private Mettod
        private void BuildURLWithParameter(int store, string lang, string currency, string query, int offset, int limit)
        {
            if (store != 0)
            {
                _context.Request.AddParameter("store", store, ParameterType.QueryString);
            }

            if (!string.IsNullOrEmpty(lang))
            {
                _context.Request.AddParameter("lang", lang, ParameterType.QueryString);
            }

            if (!string.IsNullOrEmpty(currency))
            {
                _context.Request.AddParameter("currency", currency, ParameterType.QueryString);
            }

            if (!string.IsNullOrEmpty(query))
            {
                _context.Request.AddParameter("q", query, ParameterType.QueryString);
            }

            if (offset != 0)
            {
                _context.Request.AddParameter("offset", query, ParameterType.QueryString);
            }

            if (limit != 0)
            {
                _context.Request.AddParameter("limit", limit, ParameterType.QueryString);
            }

        }
        #endregion
    }
}