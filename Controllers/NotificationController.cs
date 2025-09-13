using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _svc;
        public NotificationController(INotificationService svc) => _svc = svc;

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId) =>
            Ok(await _svc.GetByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] NotificationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(request);
            return CreatedAtAction(nameof(GetByUserId), new { userId = created.UserId }, created);
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
