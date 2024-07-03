namespace manage_auth.src.models.requests
{
    public class ResetPasswordRequest
    {
        public ResetPasswordRequest()
        {

        }

        public string Email { get; set; }
        
        public string NewPassword { get; set; }

        public string ConfirmationCode { get; set; }
    }
}