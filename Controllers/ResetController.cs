using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using System.Threading.Tasks;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetController : ControllerBase
    {
        private readonly IResetService _resetService;

        public ResetController(IResetService resetService)
        {
            _resetService = resetService;
        }

        // POST: api/reset/request
        [HttpPost("request")]
        public async Task<IActionResult> RequestReset([FromBody] ForgotRequest request)
        {
            try
            {
                var success = await _resetService.ResetRequestAsync(request.Email);
                if (success)
                    return Ok(new { message = "Reset email sent." });

                return BadRequest(new { message = "Could not process request." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // GET: api/reset/verify
        [HttpGet("verify/{code}")]
        public async Task<IActionResult> VerifyReset(string code)
        {
            try
            {
                var valid = await _resetService.ResetVerifyAsync(code);
                if (valid)
                    return Ok();

                return BadRequest(new { message = "Invalid or expired reset code." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        // POST: api/reset/password
        [HttpPost("password")]
        public async Task<IActionResult> ResetPassword([FromBody] PasswordResetRequest request)
        {
            if (request.NewPassword != request.ConfirmPassword)
                return BadRequest(new { message = "Passwords do not match." });

            try
            {
                var success = await _resetService.ResetPasswordAsync(request.ResetCode, request.NewPassword, request.ConfirmPassword);
                if (success)
                    return Ok(new { message = "Password has been reset successfully." });

                return BadRequest(new { message = "Could not reset password." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }

    
}
