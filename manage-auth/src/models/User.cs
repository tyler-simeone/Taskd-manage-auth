using Amazon.CognitoIdentityProvider.Model;

namespace manage_auth.src.models
{
    public class User : ResponseBase
    {
        public User()
        {

        }

        public int UserId { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime CreateDatetime { get; set; }

        public DateTime? UpdateDatetime { get; set; }

        public int? UpdateUserId { get; set; }
    }
}