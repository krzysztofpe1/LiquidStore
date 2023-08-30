using System.Net;

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
