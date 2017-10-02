using RestSharp;
using System;
using System.Configuration;

namespace APITesting
{
    /// <summary>
    /// This context class is created to use Context Injection
    /// for sharing data between steps even if the steps are
    /// in diffrent class files.
    /// </summary>
    public class SharedContext
    {
        public RestRequest Request;
        public IRestResponse<dynamic> Response;
        public DynamicRestClient Client;

        public SharedContext()
        { //to more commenbst
            // Reading the endpoint details from app.config file.
            var endpoint = ConfigurationManager.AppSettings["ENDPOINT"];            
            var url = String.Format("{0}/product/search/{1}/", endpoint, "V1");
            Client = new DynamicRestClient(new Uri(url));
        }
    }
}
