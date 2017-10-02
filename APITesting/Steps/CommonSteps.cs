using TechTalk.SpecFlow;

namespace APITesting
{
    using RestSharp;

    [Binding]
    public class CommonSteps 
    {
        private readonly SharedContext _context;
        public CommonSteps(SharedContext context)
        {
            _context = context; 
        }

        // Common step for all the Scenario to populate the response.
        [When(@"I get the response back from API")]
        public void WhenIGetTheResponseBackFromApi()
        {          
          _context.Response = _context.Client.Execute<dynamic>(_context.Request);         
        }

    }
}


