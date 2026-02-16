using Microsoft.AspNetCore.Mvc;
using MusicHub.Application.DTO;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _posts;
        public PostController(PostService posts)
        {
            _posts = posts;
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken ct)
        {
            var id = await _posts.CreateAsync(dto, ct);
            return Ok(new { postId = id });
        }
        [HttpPost("{postId:guid}/like")]
        public async Task<IActionResult> Like(Guid postId, [FromBody] LikePostDto dto, CancellationToken ct)
        {
            await _posts.LikeAsync(postId, dto, ct);
            return Ok();
        }
        [HttpPost("{postId:guid}/comment")]
        public async Task<IActionResult> Comment(Guid postId, [FromBody] AddCommentDto dto, CancellationToken ct)
        {
            await _posts.CommentAsync(postId, dto, ct);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetLatest([FromQuery] int take = 20, CancellationToken ct = default)
        {
            var items = await _posts.GetLatestAsync(take, ct);
            return Ok(items);
        }
    }
}
