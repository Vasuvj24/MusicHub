using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
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
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePostDto dto, CancellationToken ct)
        {
            //user gets in httpcontext attached when we get the jwt and claims are extracted from it 
            var userId = User.GetUserId();
            var id = await _posts.CreateAsync(userId,dto, ct);
            return Ok(new { postId = id });
        }
        [Authorize]
        [HttpPost("{postId:guid}/like")]
        public async Task<IActionResult> Like(Guid postId, [FromBody] LikePostDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _posts.LikeAsync(userId,postId, dto, ct);
            return Ok();
        }
        [Authorize]
        [HttpPost("{postId:guid}/comment")]
        public async Task<IActionResult> Comment(Guid postId, [FromBody] AddCommentDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _posts.CommentAsync(userId,postId, dto, ct);
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
