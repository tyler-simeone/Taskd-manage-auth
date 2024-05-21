using manage_auth.src.models.requests;
using manage_auth.src.repository;
using manage_auth.src.util;
using Microsoft.AspNetCore.Mvc;

namespace manage_auth.src.controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : Controller
    {
        IRequestValidator _validator;
        IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository, IRequestValidator requestValidator)
        {
            _validator = requestValidator;
            _authRepository = authRepository;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> GetBearerToken(int userId)
        {
            if (_validator.ValidateGetBearerToken(userId))
            {
                try
                {
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("Task ID is required.");
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult RegisterUser(CreateUser createUserRequest)
        {
            if (_validator.ValidateCreateUser(createUserRequest))
            {
                try
                {
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("CreateUserRequest is required.");
            }
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public IActionResult AuthenticateUser(AuthenticateUser authenticateUserRequest)
        {
            if (_validator.ValidateAuthenticateUser(authenticateUserRequest))
            {
                try
                {
                    return Ok();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("CreateUserRequest is required.");
            }
        }
        
        [HttpPost]
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
                    Console.WriteLine($"Error: {ex.Message}");
                    throw;
                }
            }
            else
            {
                return BadRequest("CreateUserRequest is required.");
            }
        }
    }
}