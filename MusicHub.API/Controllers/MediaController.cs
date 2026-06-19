using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.DTO;
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
        [Consumes("multipart/form-data")]
        [RequestSizeLimit(100_000_000)] // 100MB
        public async Task<IActionResult> Upload([FromForm] UploadRequest request, CancellationToken ct)
        {
            var res = await _media.UploadAsync(request.File, ct);
            return Ok(res);
        }
        [Authorize]
        [HttpDelete]
        //filename points to url now
        public async Task<IActionResult> Delete([FromQuery]string fileName, CancellationToken ct)
        {
            var result = await _media.DeleteAsync(fileName, ct);

            if (!result)
                return NotFound();

            return NoContent();
        }
    }
}
