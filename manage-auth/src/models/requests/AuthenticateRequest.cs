namespace manage_auth.src.models.requests
{
    public class AuthenticateRequest
    {
        public AuthenticateRequest()
        {

        }

        public string BearerToken { get; set; }
    }
}