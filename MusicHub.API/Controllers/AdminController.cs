using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.DTO.Common;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    public sealed class AdminController : ControllerBase
    {
        private readonly AdminService _admin;

        public AdminController(AdminService admin)
        {
            _admin = admin;
        }

        [HttpGet("reports")]
        public async Task<IActionResult> GetReports([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
        {
            var res = await _admin.GetReportsAsync(new PagedRequest { Page = page, PageSize = pageSize }, ct);
            return Ok(res);
        }

        [HttpDelete("posts/{postId:guid}")]
        public async Task<IActionResult> DeletePost(Guid postId, CancellationToken ct)
        {
            await _admin.SoftDeletePostAsync(postId, ct);
            return Ok();
        }
    }
}
