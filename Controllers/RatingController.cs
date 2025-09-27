using CebuCrust_api.Authentication;
using CebuCrust_api.Interfaces;
using CebuCrust_api.ServiceModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _svc;
        public RatingController(IRatingService svc) => _svc = svc;
        private int UserId => ClaimsHelper.GetUserId(User);

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _svc.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var r = await _svc.GetByIdAsync(id);
            return r == null ? NotFound() : Ok(r);
        }

        [HttpGet("pizza/{pizzaId:int}")]
        public async Task<IActionResult> GetByPizzaId(int pizzaId) =>
            Ok(await _svc.GetByPizzaIdAsync(pizzaId));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RatingRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(UserId, request);
            return CreatedAtAction(nameof(GetById), new { id = created.PizzaId }, created);
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