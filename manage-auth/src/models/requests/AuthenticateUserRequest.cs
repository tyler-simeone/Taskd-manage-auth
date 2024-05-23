namespace manage_auth.src.models.requests
{
    public class AuthenticateUserRequest
    {
        public AuthenticateUserRequest()
        {

        }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}