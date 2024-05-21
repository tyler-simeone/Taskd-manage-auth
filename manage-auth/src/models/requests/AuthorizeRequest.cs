namespace manage_auth.src.models.requests
{
    public class AuthorizeRequest
    {
        public AuthorizeRequest()
        {

        }

        public string ServiceName { get; set; }
        
        public string ServiceEndpoint { get; set; }
        
        public ResourceAction ResourceAction { get; set; }
    }
}