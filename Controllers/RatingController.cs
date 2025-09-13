using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _svc;
        public RatingController(IRatingService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _svc.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RatingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = created.RatingId }, created);
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] RatingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var updated = await _svc.UpdateAsync(id, request);
            return updated == null ? NotFound() : Ok(updated);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }

    
}
