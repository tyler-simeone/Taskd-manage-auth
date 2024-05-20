using manage_auth.src.dataservice;
using manage_auth.src.models;

namespace manage_auth.src.repository
{
    public class AuthRepository : IAuthRepository
    {
        IAuthDataservice _AuthDataservice;

        public AuthRepository(IAuthDataservice AuthDataservice)
        {
            _AuthDataservice = AuthDataservice;
        }

    }
}