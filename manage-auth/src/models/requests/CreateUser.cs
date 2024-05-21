namespace manage_auth.src.models.requests
{
    public class CreateUser
    {
        public CreateUser()
        {

        }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }
    }
}