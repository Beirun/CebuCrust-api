// Controllers/UserController.cs
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CebuCrust_api.ServiceModels;
using CebuCrust_api.Interfaces;
using System.Threading.Tasks;

namespace CebuCrust_api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _svc;
        public UserController(IUserService svc) => _svc = svc;

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAll() =>
            Ok(await _svc.GetAllAsync());

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromForm] UserUpdateRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            Console.WriteLine($"Update request for ID: {id}");
            Console.WriteLine($"FirstName: {request.UserFName}");
            Console.WriteLine($"LastName: {request.UserLName}");
            Console.WriteLine($"Email: {request.UserEmail}");
            Console.WriteLine($"PhoneNo: {request.UserPhoneNo}");
            Console.WriteLine($"CurrentPassword: {request.CurrentPassword}");
            Console.WriteLine($"NewPassword: {request.NewPassword}");
            Console.WriteLine($"ConfirmPassword: {request.ConfirmPassword}");
            Console.WriteLine($"Image: {(request.Image != null ? request.Image.FileName : "No image")}");
            try
            {
                if (request.Image != null)
                    await _svc.SaveImageAsync(id, request.Image);

                var updated = await _svc.UpdateAsync(id, request);

                if (updated == null) return NotFound();

                

                return Ok(updated);
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id) =>
            await _svc.DeleteAsync(id) ? NoContent() : NotFound();
    }
}
