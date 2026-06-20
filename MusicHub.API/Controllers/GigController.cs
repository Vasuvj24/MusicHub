using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
using MusicHub.Application.DTO;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/gigs")]
    public class GigController : ControllerBase
    {
        private readonly GigService _gigs;
        public GigController(GigService gigs)
        {
            _gigs = gigs;
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateGigDto gigDto, CancellationToken cts)
        {
            var userId = User.GetUserId();
            var id = await _gigs.CreateAsync(userId, gigDto,cts);
            return Ok(new {gigId =  id});
        }
        [Authorize]
        [HttpPost("{gigId:guid}/apply")]
        public async Task<IActionResult> Apply(Guid gigId, [FromBody] ApplyToGigDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _gigs.ApplyAsync(userId, gigId, dto, ct);
            return Ok();
        }
        // Only creator can approve (enforced in domain)
        [Authorize]
        [HttpPost("{gigId:guid}/approve")]
        public async Task<IActionResult> Approve(Guid gigId, [FromBody] ApproveMemberDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _gigs.ApproveAsync(userId, gigId, dto, ct);
            return Ok();
        }
        //doesn't require authorization
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] int take = 20, CancellationToken ct = default)
        {
            var gigs = await _gigs.GetLatestAsync(take, ct);
            return Ok(gigs); // (later: return DTO, like Posts)
        }
        [HttpGet("paged")]
        public async Task<IActionResult>GetPaged([FromQuery] int page = 1,[FromQuery] int pageSize = 20,CancellationToken ct = default)
        {
            var result =
                await _gigs.GetPagedAsync(
                    page,
                    pageSize,
                    ct);

            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{gigId:guid}")]
        public async Task<IActionResult>Delete(Guid gigId,CancellationToken ct)
        {
            var userId = User.GetUserId();

            await _gigs.DeleteAsync(
                userId,
                gigId,
                ct);

            return NoContent();
        }
    }
}
