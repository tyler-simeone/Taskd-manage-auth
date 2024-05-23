namespace manage_auth.src.models.requests
{
    public class ConfirmUserRequest
    {
        public ConfirmUserRequest()
        {

        }

        public string Email { get; set; }

        public string ConfirmationCode { get; set; }
    }
}