using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
using MusicHub.Application.DTO.Auth;
using System.Security.Claims;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/me")]
    public class MeController : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public IActionResult Get()
        {
            var userId = User.GetUserId();
            var email = User.FindFirstValue(ClaimTypes.Email) ?? "";
            var role = User.FindFirstValue(ClaimTypes.Role) ?? "";

            return Ok(new MeDto
            {
                UserId = userId,
                Email = email,
                Role = role
            });
        }
    }
}
