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
    public class CartController : ControllerBase
    {
        private readonly ICartService _svc;
        public CartController(ICartService svc) => _svc = svc;

        [HttpGet("user/{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId) =>
            Ok(await _svc.GetByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(request.UserId, request.PizzaId, request.Quantity);
            return CreatedAtAction(nameof(GetByUserId), new { userId = created.UserId }, created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(request.UserId, request.PizzaId, request.Quantity);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete([FromBody] CartRequest request) =>
            await _svc.DeleteAsync(request.UserId, request.PizzaId) ? NoContent() : NotFound();
    }

    
}
