using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.DTO;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserService _Service;
        public AuthController(UserService service)
        {
            _Service = service;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterUserDto dto)
        {
            await _Service.RegisterAsync(dto.Email, dto.Password);
            return Ok("registered succesfully");
        }
        [HttpPost("logic")]
        public async Task<IActionResult> login([FromBody] LoginDto dto)
        {
            var res = await _Service.LoginAsync(dto);
            return Ok(res);
        }
    }
}
