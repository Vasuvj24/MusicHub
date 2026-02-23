using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/media")]
    public sealed class MediaController : ControllerBase
    {
        private readonly MediaService _media;
        public MediaController(MediaService media)
        {
            _media = media;
        }
        [Authorize]
        [HttpPost("upload")]
        [RequestSizeLimit(100_000_000)] // 100MB
        public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken ct)
        {
            var res = await _media.UploadAsync(file, ct);
            return Ok(res);
        }
        [Authorize]
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> Delete(string fileName, CancellationToken ct)
        {
            var result = await _media.DeleteAsync(fileName, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
