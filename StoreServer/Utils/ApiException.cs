using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StoreServer.Utils
{
    internal class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public ApiException(HttpStatusCode code, string message) :
        base(message)
        {
            StatusCode = code;
        }
    }
}
