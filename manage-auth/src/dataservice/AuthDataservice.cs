using System;
using manage_auth.src.models;
using MySql.Data.MySqlClient;

namespace manage_auth.src.dataservice
{
    public class AuthDataservice : IAuthDataservice
    {
        private IConfiguration _configuration;

        public AuthDataservice(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        #region HELPERS

        #endregion
    }
}