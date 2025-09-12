// Controllers/OrderController.cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.Models;
using CebuCrust_api.Services;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _svc;
        public OrderController(IOrderService svc) => _svc = svc;

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId) =>
            Ok(await _svc.GetByUserIdAsync(userId));


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var order = new Order
            {
                UserId = request.UserId,
                LocationId = request.LocationId,
                OrderInstruction = request.OrderInstruction,
                OrderEstimate = request.OrderEstimate,
                OrderStatus = request.OrderStatus
            };

            var created = await _svc.CreateAsync(order, request.Items);
            return CreatedAtAction(nameof(GetByUserId), new { userId = created.UserId }, created);
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
