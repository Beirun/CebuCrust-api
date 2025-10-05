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
    public class CartController : ControllerBase
    {
        private readonly ICartService _svc;
        public CartController(ICartService svc) => _svc = svc;

        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet("user")]
        public async Task<IActionResult> GetByUser() =>
            Ok(await _svc.GetByUserAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CartRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(UserId, request);
            return Ok(created);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] CartRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(UserId, request);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{pizzaId:int}")]
        public async Task<IActionResult> Delete(int pizzaId) =>
            await _svc.DeleteCartItemAsync(UserId, pizzaId) ? NoContent() : NotFound();
            
        [HttpDelete]
        public async Task<IActionResult> DeleteAll() =>
            await _svc.DeleteCartAsync(UserId) ? NoContent() : NotFound();
    }
}
