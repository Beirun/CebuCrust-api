using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.Models;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _svc;
        public AccountController(IAccountService svc) => _svc = svc;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var u = new User
            {
                UserFName = request.FirstName,
                UserLName = request.LastName,
                UserEmail = request.Email,
                UserPhoneNo = request.PhoneNumber
            };

            try
            {
                var res = await _svc.RegisterAsync(u, request.Password, request.ConfirmPassword);
                SetRefreshCookie(res.RefreshToken);
                return Ok(new { token = res.AccessToken, user = res.User, message= "Registered Successfully" });
            }
            catch (Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                var res = await _svc.LoginAsync(request.Email, request.Password);
                SetRefreshCookie(res.RefreshToken);
                return Ok(new { token = res.AccessToken, user = res.User, message = "Logged In Successfully" });
            }catch(Exception ex) { return BadRequest(new { message = ex.Message }); }
        }

        [HttpPost("refresh")]
        public IActionResult Refresh()
        {
            var cookie = Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(cookie)) return Unauthorized(new { message = "No refresh token" });

            var newAccess = _svc.Refresh(cookie);
            if (newAccess == null) return Unauthorized(new { message = "Invalid or expired refresh token" });

            return Ok(new { token = newAccess });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {

            if (Request.Cookies.ContainsKey("refreshToken"))
            {
                Response.Cookies.Delete("refreshToken");
            }
            return Ok(new { message = "Logged Out Successfully" });
        }

        private void SetRefreshCookie(string token)
        {

            Response.Cookies.Append("refreshToken", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(7)
            });
        }
    }

}
