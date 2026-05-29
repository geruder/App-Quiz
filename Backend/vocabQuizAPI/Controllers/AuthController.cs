using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using vocabQuizAPI.Repositories;
using vocabQuizAPI.Models.Dtos;

namespace vocabQuizAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;

        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto request)
        {
            var user = await _authRepository.RegisterAsync(request);
            if (user == null)
            {
                return BadRequest(new { message = "Email already exists" });
            }
            return Ok(new { message = "Registration successful", userId = user.UserId, username = user.Username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto request)
        {
            var user = await _authRepository.LoginAsync(request);
            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email or password" });
            }

            //simdilik jwt yok, ilerleyen kisimlarda belki
            return Ok(new { message = "Login successful", userId = user.UserId, username = user.Username });
        }

    }
}
