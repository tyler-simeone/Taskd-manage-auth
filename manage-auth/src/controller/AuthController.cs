using System.Net;
using System.Net.Http.Headers;
using manage_auth.src.clients;
using manage_auth.src.models;
using manage_auth.src.models.errors;
using manage_auth.src.models.requests;
using manage_auth.src.util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace manage_auth.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        IRequestValidator _validator;
        IConfiguration _configuration;
        ICognitoClient _cognitoClient;
        IUsersClient _usersClient;

        public AuthController(IRequestValidator requestValidator, 
                              IConfiguration configuration,
                              ICognitoClient cognitoClient, 
                              IUsersClient usersClient)
        {
            _validator = requestValidator;
            _configuration = configuration;
            _cognitoClient = cognitoClient;
            _usersClient = usersClient;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetBearerToken(int userId)
        {
            try
            {
                var bearerToken = await GetBearerToken();
                return Ok(bearerToken);
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        [HttpPost("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AuthorizeRequest(AuthorizeRequest authorizeRequestRequest)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        [HttpPost("user/authenticate")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserRequest authenticateUserRequest)
        {
            try
            {
                var authResponse = await _cognitoClient.AuthenticateUserAsync(authenticateUserRequest);
                var authenticatedUser = await _usersClient.GetUser(authenticateUserRequest.Email, authResponse.AuthenticationResult.AccessToken);
                var response = new AuthenticationResponse()
                {
                    AuthenticationResult = authResponse.AuthenticationResult,
                    Status = authResponse.HttpStatusCode,
                    User = authenticatedUser
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterUser(CreateUserRequest createUserRequest)
        {
            try
            {
                await _cognitoClient.SignUpUserAsync(createUserRequest.Email, createUserRequest.Password, createUserRequest.FirstName, createUserRequest.LastName);
                var bearerToken = await GetBearerToken();
                await _usersClient.CreateUser(createUserRequest, bearerToken.access_token);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }
        
        [HttpPost("user/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            try
            {
                var confirmSignUpResponse = await _cognitoClient.ConfirmUserAsync(confirmUserRequest);
                return Ok();
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        [HttpPost("user/initiateresetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> InititeResetPassword(string email)
        {
            try
            {
                var resetPasswordResponse = await _cognitoClient.ResetPasswordAsync(email);
                return Ok(resetPasswordResponse);
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }
        
        [HttpPost("user/resetpassword")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest resetPasswordRequest)
        {
            try
            {
                var resetPasswordResponse = await _cognitoClient.ConfirmResetPasswordAsync(resetPasswordRequest);
                return Ok(resetPasswordResponse);
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        [HttpPost("user/resendconfirmationcode")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ResendConfirmationCode(string email)
        {
            try
            {
                var resendConfirmationCodeResponse = await _cognitoClient.ResendConfirmationCodeAsync(email);
                return Ok(resendConfirmationCodeResponse);
            }
            catch (Exception ex)
            {
                return InternalError(ex.Message);
            }
        }

        #region HELPERS

        private async Task<TokenResponse> GetBearerToken()
        {
            var clientId = _configuration["ClientId"];
            if (clientId.IsNullOrEmpty())
                clientId = _configuration["AWS:Cognito:ClientId"];

            var clientSecret = _configuration["ClientSecret"];
            if (clientSecret.IsNullOrEmpty())
                clientSecret = _configuration["AWS:Cognito:ClientSecret"];

            var tokenEndpoint = _configuration["TokenEndpoint"];
            if (tokenEndpoint.IsNullOrEmpty())
                tokenEndpoint = _configuration["AWS:Cognito:TokenEndpoint"];

            var client = new HttpClient();

            var content = new FormUrlEncodedContent(
            [
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("client_id", clientId),
                new KeyValuePair<string, string>("client_secret", clientSecret)
            ]);

            content.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");
            
            var response = await client.PostAsync(tokenEndpoint, content);
            
            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TokenResponse>(responseBody);
            }
            else
            {
                throw new Exception(response.ReasonPhrase);
            }
        }
        
        private ObjectResult InternalError(string message)
        {
            var errorResponse = new ErrorResponse
            {
                Status = HttpStatusCode.InternalServerError,
                Type = "InternalServerError",
                Detail = message
            };
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);
        }

        #endregion 
    }
}