using System;
using System.Net.Http;
using System.Threading.Tasks;
using manage_auth.src.models;
using System.Net.Http.Formatting;
using Newtonsoft.Json;
using Amazon.CognitoIdentityProvider;
using Amazon.CognitoIdentityProvider.Model;
using Microsoft.Extensions.Configuration;
using Amazon;
using Amazon.Runtime;
using System.Text;
using System.Security.Cryptography;
using Amazon.Extensions.CognitoAuthentication;
using manage_auth.src.models.requests;

namespace manage_auth.src.clients
{
    public class CognitoClient : ICognitoClient
    {
        private readonly string _userPoolId;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _accessKey;
        private readonly string _secretAccessKey;
        private readonly AmazonCognitoIdentityProviderClient _cognitoClient;

        public CognitoClient(IConfiguration configuration)
        {
            _userPoolId = configuration["AWS:Cognito:UserPoolId"];
            _clientId = configuration["AWS:Cognito:ClientId"];
            _clientSecret = configuration["AWS:Cognito:ClientSecret"];
            _accessKey = configuration["AWS:Cognito:AccessKey"];
            _secretAccessKey = configuration["AWS:Cognito:SecretAccessKey"];

            var credentials = new BasicAWSCredentials(_accessKey, _secretAccessKey);
            _cognitoClient = new AmazonCognitoIdentityProviderClient(credentials, RegionEndpoint.USEast1);
        }

        public async Task SignUpUserAsync(string email, string password, string firstName, string lastName)
        {
            var secretHash = GetSecretHash(email, _clientId, _clientSecret);
            var signUpRequest = new SignUpRequest
            {
                ClientId = _clientId,
                SecretHash = secretHash,
                Username = email,
                Password = password,
                UserAttributes = new List<AttributeType>
                {
                    new AttributeType { Name = "email", Value = email },
                    new AttributeType { Name = "name", Value = firstName + " " + lastName }
                }
            };

            await _cognitoClient.SignUpAsync(signUpRequest);
        }
        
        public async Task<InitiateAuthResponse> AuthenticateUserAsync(AuthenticateUserRequest authUserRequest)
        {
            var authRequest = new InitiateAuthRequest
            {
                AuthFlow = AuthFlowType.USER_PASSWORD_AUTH,
                ClientId = _clientId,
                AuthParameters = new Dictionary<string, string>
                {
                    { "USERNAME", authUserRequest.Email },
                    { "PASSWORD", authUserRequest.Password },
                    { "SECRET_HASH", GetSecretHash(authUserRequest.Email, _clientId, _clientSecret) }
                }
            };

            try
            {
                var authResponse = await _cognitoClient.InitiateAuthAsync(authRequest).ConfigureAwait(false);

                Console.WriteLine("Authentication successful");
                Console.WriteLine($"ID Token: {authResponse.AuthenticationResult.IdToken}");
                Console.WriteLine($"Access Token: {authResponse.AuthenticationResult.AccessToken}");
                Console.WriteLine($"Refresh Token: {authResponse.AuthenticationResult.RefreshToken}");

                return authResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error authenticating user: {e.Message}");
                throw e;
            }
        }
        
        public async Task<ConfirmSignUpResponse> ConfirmUserAsync(ConfirmUserRequest confirmUserRequest)
        {
            var confirmRequest = new ConfirmSignUpRequest
            {
                ClientId = _clientId,
                Username = confirmUserRequest.Email,
                ConfirmationCode = confirmUserRequest.ConfirmationCode,
                SecretHash = GetSecretHash(confirmUserRequest.Email, _clientId, _clientSecret)
            };

            try
            {
                var authResponse = await _cognitoClient.ConfirmSignUpAsync(confirmRequest).ConfigureAwait(false);
                return authResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error authenticating user: {e.Message}");
                throw e;
            }
        }

        #region HELPERS

        private string GetSecretHash(string username, string clientId, string clientSecret)
        {
            string message = username + clientId;
            var key = Encoding.UTF8.GetBytes(clientSecret);
            var msg = Encoding.UTF8.GetBytes(message);
            using (var hmac = new HMACSHA256(key))
            {
                var hash = hmac.ComputeHash(msg);
                return Convert.ToBase64String(hash);
            }
        }

        #endregion
    }
}