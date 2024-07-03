using manage_auth.src.models.requests;

namespace manage_auth.src.util
{
    public interface IRequestValidator
    {
        bool ValidateGetBearerToken(int userId);
        
        bool ValidateAuthorizeRequest(AuthorizeRequest authorizeRequestRequest);
        
        bool ValidateAuthenticateRequest(AuthenticateRequest authenticateRequestRequest);

        bool ValidateCreateUser(CreateUserRequest createUserRequest);
        
        bool ValidateResetPassword(string email);

        bool ValidateConfirmResetPassword(ResetPasswordRequest resetPasswordRequest);

        bool ValidateResendConfirmationCode(string email);

        bool ValidateConfirmUser(ConfirmUserRequest confirmUserRequest);

        bool ValidateAuthenticateUser(AuthenticateUserRequest authenticateUserRequest);

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

        public bool ValidateCreateUser(CreateUserRequest createUserRequest)
        {
            return true;
        }
        
        public bool ValidateResetPassword(string email)
        {
            return true;
        }
        
        public bool ValidateConfirmResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            return true;
        }
        
        public bool ValidateResendConfirmationCode(string email)
        {
            return true;
        }

        public bool ValidateConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            return true;
        }
        
        public bool ValidateAuthenticateUser(AuthenticateUserRequest authenticateUserRequest)
        {
            return true;
        }
    }
}