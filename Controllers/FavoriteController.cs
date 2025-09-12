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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _svc;
        public FavoriteController(IFavoriteService svc) => _svc = svc;

        [HttpGet("{userId:int}")]
        public async Task<IActionResult> GetByUserId(int userId) =>
            Ok(await _svc.GetByUserIdAsync(userId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(request);
            return CreatedAtAction(nameof(GetByUserId), new { userId = created.UserId }, created);
        }

        [HttpDelete("{userId:int}/{pizzaId:int}")]
        public async Task<IActionResult> Delete(int userId, int pizzaId) =>
            await _svc.DeleteAsync(userId, pizzaId) ? NoContent() : NotFound();
    }

    
}
