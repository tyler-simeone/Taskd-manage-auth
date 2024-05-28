using manage_auth.src.models;
using manage_auth.src.models.requests;

namespace manage_auth.src.clients
{
    public interface IUsersClient
    { 
        public Task<HttpResponseMessage> CreateUser(CreateUserRequest createUserRequest, string bearerToken);
        
        public Task<User> GetUser(string email, string bearerToken);
    }
}