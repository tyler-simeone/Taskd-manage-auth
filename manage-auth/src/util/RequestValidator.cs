using manage_auth.src.models.requests;

namespace manage_auth.src.util
{
    public interface IRequestValidator
    {
        bool ValidateGetBearerToken(int userId);

        bool ValidateCreateUser(CreateUser createUserRequest);
        
        bool ValidateAuthenticateUser(AuthenticateUser authenticateUserRequest);

        bool ValidateAuthorizeRequest(AuthorizeRequest authorizeRequestRequest);
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

        public bool ValidateCreateUser(CreateUser createUserRequest)
        {
            return true;
        }
        
        public bool ValidateAuthenticateUser(AuthenticateUser authenticateUserRequest)
        {
            return true;
        }
        
        public bool ValidateAuthorizeRequest(AuthorizeRequest authorizeRequestRequest)
        {
            return true;
        }
    }
}