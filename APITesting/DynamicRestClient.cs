using System;
namespace APITesting
{
    using System.Diagnostics.CodeAnalysis;

    using RestSharp;
    using Newtonsoft.Json;

    using RestSharp.Deserializers;

    public class DynamicRestClient : RestClient
    {
        public DynamicRestClient()
        {
            this.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        [SuppressMessage("Microsoft.Design", "CA1054:UriParametersShouldNotBeStrings", MessageId = "0#")]
        public DynamicRestClient(Uri baseUrl)
            : base(baseUrl)
        {
            this.AddHandler("application/json", new DynamicJsonDeserializer());
        }

        private class DynamicJsonDeserializer : IDeserializer
        {
            #region Public Properties

            public string DateFormat { get; set; }

            public string Namespace { get; set; }

            public string RootElement { get; set; }

            #endregion

            #region Public Methods and Operators

            public T Deserialize<T>(IRestResponse response)
            {
                if (response == null)
                {
                    throw new ArgumentNullException("response");
                }

                return JsonConvert.DeserializeObject<dynamic>(response.Content);
            }

            #endregion
        }
    }
}
