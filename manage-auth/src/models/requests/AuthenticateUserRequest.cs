namespace manage_auth.src.models.requests
{
    public class AuthenticateUser
    {
        public AuthenticateUser()
        {

        }

        public string Email { get; set; }

        public string Password { get; set; }
    }
}