using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
using MusicHub.Application.DTO;
using MusicHub.Application.DTO.Auth;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _auth;
        public AuthController(AuthService auth)
        {
            _auth = auth;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserDto dto,CancellationToken cts)
        {
            await _auth.RegisterAsync(dto, cts);
            return Ok("registered succesfully");
        }
        [HttpPost("login")]
        public async Task<IActionResult> login([FromBody] LoginDto dto,CancellationToken cts)
        {
            var res = await _auth.LoginAsync(dto,cts);
            return Ok(res);
        }
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshRequestDto dto, CancellationToken ct)
        {
            var res = await _auth.RefreshAsync(dto, ct);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _auth.LogoutAsync(userId, ct);
            return Ok();
        }
    }
}
