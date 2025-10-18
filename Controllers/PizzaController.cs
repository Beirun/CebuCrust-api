using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PizzaController : ControllerBase
    {
        private readonly IPizzaService _svc;
        public PizzaController(IPizzaService svc) => _svc = svc;

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var pizza = await _svc.GetByIdAsync(id);
            return pizza == null ? NotFound() : Ok(pizza);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromForm] PizzaRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var created = await _svc.CreateAsync(request);

            if (request.Image != null)
                await _svc.SaveImageAsync(created.PizzaId, request.Image);

            return CreatedAtAction(nameof(GetById), new { id = created.PizzaId }, created);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromForm] PizzaRequest request)
        {
            if (request.Image != null)
                await _svc.SaveImageAsync(id, request.Image);
            var updated = await _svc.UpdateAsync(id, request);
            if (updated == null) return NotFound();

            

            return Ok(updated);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}