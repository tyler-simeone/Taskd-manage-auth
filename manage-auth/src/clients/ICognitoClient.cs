using Amazon.CognitoIdentityProvider.Model;
using manage_auth.src.models.requests;

namespace manage_auth.src.clients
{
    public interface ICognitoClient
    { 
        public Task SignUpUserAsync(string email, string password, string firstName, string lastName);

        public Task<InitiateAuthResponse> AuthenticateUserAsync(AuthenticateUserRequest authUserRequest);

        public Task<ConfirmSignUpResponse> ConfirmUserAsync(ConfirmUserRequest confirmUserRequest);

        public Task<ResendConfirmationCodeResponse> ResendConfirmationCodeAsync(string email);
        
        public Task<ForgotPasswordResponse> ResetPasswordAsync(string email);

        public Task<ConfirmForgotPasswordResponse> ConfirmResetPasswordAsync(ResetPasswordRequest resetPasswordRequest);
    }
}