using manage_auth.src.models;
using manage_auth.src.repository;
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
        public async Task<ActionResult> GetTask(int id, int userId)
        {
            if (_validator.ValidateGetTask(id, userId))
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
        public IActionResult CreateTask(CreateTask createTaskRequest)
        {
            if (_validator.ValidateCreateTask(createTaskRequest))
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
                return BadRequest("CreateTaskRequest is required.");
            }
        }
    }
}