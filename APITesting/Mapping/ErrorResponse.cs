using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITesting.Mapping
{
    /// <summary>
    /// Mapping class created to cast the error code returned in response.
    /// </summary>
   public class ErrorResponse
    {
        public string errorCode { get; set; }
        public string message { get; set; }
    }
}
