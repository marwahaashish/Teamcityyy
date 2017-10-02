using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITesting.ScenarioContextEnum
{
    /// <summary>
    /// Enum created for various error code returned by API
    /// </summary>
  public enum ErrorCodeEnum
    {
        ERROR_MISSINGSTORE,
        ERROR_MISSINGLANGUAGE,
        ERROR_MISSINGCURRENCY,
        ERROR_MISSINGQUERY,
        ERROR_INVALIDSTORE,
        ERROR_INVALIDLANGUAGE,
        ERROR_INVALIDCURRENCYLENGTH,
    }
}
