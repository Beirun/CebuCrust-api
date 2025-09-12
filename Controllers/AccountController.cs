using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.Models;
using CebuCrust_api.Services;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _svc;

        public AccountController(IAccountService svc)
        {
            _svc = svc;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var u = new User
            {
                UserFName = request.FirstName,
                UserLName = request.LastName,
                UserEmail = request.Email,
                UserPhoneNo = request.PhoneNo,
            };

            try
            {
                var result = await _svc.RegisterAsync(u, request.Password);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _svc.LoginAsync(request.Email, request.Password);
            if (result == null) return Unauthorized(new { message = "Invalid credentials" });
            return Ok(result);
        }
    }

    public class RegisterRequest
    {
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";
        public string? PhoneNo { get; set; }
        public string Password { get; set; } = "";
    }

    public class LoginRequest
    {
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
    }
}
