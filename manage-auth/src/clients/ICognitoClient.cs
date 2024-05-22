using manage_auth.src.models;

namespace manage_auth.src.clients
{
    public interface ICognitoClient
    { 
        public Task SignUpUserAsync(string email, string password, string firstName, string lastName);
    }
}