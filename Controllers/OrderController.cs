using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;
using CebuCrust_api.Authentication;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _svc;
        public OrderController(IOrderService svc) => _svc = svc;

        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet("me")]
        public async Task<IActionResult> GetByUser() =>
            Ok(await _svc.GetByUserAsync(UserId));

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(UserId, request);
            return Ok(created);
        }

        [HttpPut("{orderId:int}")]
        public async Task<IActionResult> Update(int orderId, [FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _svc.UpdateAsync(orderId, request);
            return updated == null ? NotFound() : Ok(updated);
        }
        
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(int orderId, [FromBody] string status)
        {
            if (string.IsNullOrEmpty(status)) return BadRequest("Status cannot be empty.");
            var updated = await _svc.UpdateStatusAsync(orderId, status);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{orderId:int}")]
        public async Task<IActionResult> Delete(int orderId) =>
            await _svc.DeleteAsync(orderId) ? NoContent() : NotFound();
    }
}
