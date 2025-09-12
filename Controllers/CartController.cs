// Controllers/CartController.cs
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
        public async Task<IActionResult> Create([FromBody] Cart cart)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var created = await _svc.CreateAsync(cart);
            return CreatedAtAction(nameof(GetByUserId), new { userId = created.UserId }, created);
        }

        [HttpPut("{userId:int}/{pizzaId:int}")]
        public async Task<IActionResult> Update(int userId, int pizzaId, [FromBody] Cart cart)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);
            var updated = await _svc.UpdateAsync(userId, pizzaId, cart);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{userId:int}/{pizzaId:int}")]
        public async Task<IActionResult> Delete(int userId, int pizzaId) =>
            await _svc.DeleteAsync(userId, pizzaId) ? NoContent() : NotFound();
    }
}
