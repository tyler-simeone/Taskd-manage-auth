using manage_auth.src.models.requests;

namespace manage_auth.src.util
{
    public interface IRequestValidator
    {
        bool ValidateGetBearerToken(int userId);
        
        bool ValidateAuthorizeRequest(AuthorizeRequest authorizeRequestRequest);
        
        bool ValidateAuthenticateRequest(AuthenticateRequest authenticateRequestRequest);

        bool ValidateCreateUser(CreateUser createUserRequest);
        
        bool ValidateAuthenticateUser(AuthenticateUser authenticateUserRequest);

    }

    public class RequestValidator : IRequestValidator
    {
        public RequestValidator()
        {

        }

        public bool ValidateGetBearerToken(int userId)
        {
            return true;
        }

        public bool ValidateAuthenticateRequest(AuthenticateRequest authenticateRequestRequest)
        {
            return true;
        }

        public bool ValidateAuthorizeRequest(AuthorizeRequest authorizeRequestRequest)
        {
            return true;
        }

        public bool ValidateCreateUser(CreateUser createUserRequest)
        {
            return true;
        }
        
        public bool ValidateAuthenticateUser(AuthenticateUser authenticateUserRequest)
        {
            return true;
        }
    }
}