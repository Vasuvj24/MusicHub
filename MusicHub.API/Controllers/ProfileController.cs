using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
using MusicHub.Application.DTO.Profile;
using MusicHub.Application.Services;
using MusicHub.Domain.Users;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/profile")]
    public class ProfileController : ControllerBase
    {
        private readonly ProfileService _profiles;

        public ProfileController(ProfileService profiles)
        {
            _profiles = profiles;
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfileDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _profiles.UpdateAsync(userId, dto, ct);
            return Ok();
        }

        [Authorize]
        [HttpPost("services")]
        public async Task<IActionResult> AddService([FromBody] AddServiceDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _profiles.AddServiceAsync(userId, dto, ct);
            return Ok();
        }

        [AllowAnonymous]
        [HttpGet("{userId:guid}")]
        public async Task<IActionResult> Get(Guid userId, CancellationToken ct)
        {
            var profile = await _profiles.GetAsync(userId, ct);
            return Ok(profile);
        }
    }
}
