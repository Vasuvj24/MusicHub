using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/feed")]
    public class FeedController : ControllerBase
    {
        private readonly PostService _posts;

        public FeedController(PostService posts)
        {
            _posts = posts;
        }

        [HttpGet]
        public async Task<IActionResult>
        GetFeed(
            int take = 20,
            CancellationToken ct = default)
        {
            var feed =
                await _posts.GetFeedAsync(
                    take,
                    ct);

            return Ok(feed);
        }
    }
}
