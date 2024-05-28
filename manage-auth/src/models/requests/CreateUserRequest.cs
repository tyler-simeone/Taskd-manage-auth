namespace manage_auth.src.models.requests
{
    public class CreateUserRequest
    {
        public CreateUserRequest()
        {

        }

        public string Email { get; set; }
        
        public string Password { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}