using System.Net;

namespace manage_auth.src.models
{
    public class ResponseBase
    {
        public HttpStatusCode Status { get; set; }
        public string Type { get; set; }
        public string Detail { get; set; }
    }
}