using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _svc;
        public NotificationController(INotificationService svc) => _svc = svc;

        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet("me")]
        public async Task<IActionResult> GetByUser() =>
            Ok(await _svc.GetByUserAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(UserId, request);
            return Ok(created);
        }

        [HttpPut("{id:int}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status)) return BadRequest("Status cannot be empty.");
            var updated = await _svc.UpdateStatusAsync(id, status);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
