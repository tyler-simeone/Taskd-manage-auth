using System.Net;
using System.Net.Http.Headers;
using manage_auth.src.clients;
using manage_auth.src.models;
using manage_auth.src.models.errors;
using manage_auth.src.models.requests;
using manage_auth.src.util;
using Microsoft.AspNetCore.Mvc;
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
            if (_validator.ValidateGetBearerToken(userId))
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
            else
            {
                return BadRequest("Task ID is required.");
            }
        }

        [HttpPost("authorize")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AuthorizeRequest(AuthorizeRequest authorizeRequestRequest)
        {
            if (_validator.ValidateAuthorizeRequest(authorizeRequestRequest))
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
            else
            {
                return BadRequest("CreateUserRequest is required.");
            }
        }

        [HttpPost("user/authenticate")]
        [ProducesResponseType(typeof(AuthenticationResponse), StatusCodes.Status200OK)]
        public async Task<IActionResult> AuthenticateUser(AuthenticateUserRequest authenticateUserRequest)
        {
            if (_validator.ValidateAuthenticateUser(authenticateUserRequest))
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
            else
            {
                return BadRequest("AuthenticateUserRequest is required.");
            }
        }

        [HttpPost("user")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> RegisterUser(CreateUserRequest createUserRequest)
        {
            if (_validator.ValidateCreateUser(createUserRequest))
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
            else
            {
                return BadRequest("CreateUserRequest is required.");
            }
        }
        
        [HttpPost("user/confirm")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> ConfirmUser(ConfirmUserRequest confirmUserRequest)
        {
            if (_validator.ValidateConfirmUser(confirmUserRequest))
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
            else
            {
                return BadRequest("ConfirmUserRequest is required.");
            }
        }

        #region HELPERS

        private async Task<TokenResponse> GetBearerToken()
        {
            var clientId = _configuration["AWS:Cognito:ClientId"];
            var clientSecret = _configuration["AWS:Cognito:ClientSecret"];
            var tokenEndpoint = _configuration["AWS:Cognito:TokenEndpoint"];

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
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseBody);
                return tokenResponse;
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