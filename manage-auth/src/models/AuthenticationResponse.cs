using Amazon.CognitoIdentityProvider.Model;

namespace manage_auth.src.models
{
    public class AuthenticationResponse : ResponseBase
    {
        public AuthenticationResponse()
        {
            Type = "AuthenticationResponse";
        }
        
        public AuthenticationResultType AuthenticationResult { get; set; }
        public User User { get; set; }
    }
}