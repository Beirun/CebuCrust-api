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
    public class LocationController : ControllerBase
    {
        private readonly ILocationService _svc;
        public LocationController(ILocationService svc) => _svc = svc;

        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet("me")]
        public async Task<IActionResult> GetByUser()
        {
            var locations = await _svc.GetByUserAsync(UserId);
            return Ok(locations);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var loc = await _svc.CreateAsync(UserId, request);
            return CreatedAtAction(nameof(GetByUser), loc);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] LocationRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(UserId, id, request);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(UserId, id) ? NoContent() : NotFound();
    }
}
