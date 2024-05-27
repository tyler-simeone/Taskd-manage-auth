using Amazon.CognitoIdentityProvider.Model;

namespace manage_auth.src.models
{
    public class AuthResponse : ResponseBase
    {
        public AuthResponse()
        {
            Type = "AuthResponse";
        }
        public AuthenticationResultType AuthenticationResult { get; set; }
    }
}