using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/search")]
    public class SearchController : ControllerBase
    {
        private readonly PostService _postService;
        private readonly GigService _gigService;

        public SearchController(
            PostService postService,
            GigService gigService)
        {
            _postService = postService;
            _gigService = gigService;
        }

        [HttpGet("posts")]
        public async Task<IActionResult>
            SearchPosts([FromQuery] string q)
        {
            var result =
                await _postService.SearchPostsAsync(q);

            return Ok(result);
        }

        [HttpGet("gigs")]
        public async Task<IActionResult>
            SearchGigs([FromQuery] string q)
        {
            var result =
                await _gigService.SearchGigsAsync(q);

            return Ok(result);
        }
    }
}
