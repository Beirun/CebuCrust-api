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
    public class FavoriteController : ControllerBase
    {
        private readonly IFavoriteService _svc;
        public FavoriteController(IFavoriteService svc) => _svc = svc;

        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet("me")]
        public async Task<IActionResult> GetByUser() =>
            Ok(await _svc.GetByUserAsync(UserId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] FavoriteRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var success = await _svc.CreateAsync(UserId, request);
            return success ? Ok(request) : Conflict("Favorite already exists");
        }

        [HttpDelete("{pizzaId:int}")]
        public async Task<IActionResult> Delete(int pizzaId) =>
            await _svc.DeleteAsync(UserId, pizzaId) ? NoContent() : NotFound();
    }
}
