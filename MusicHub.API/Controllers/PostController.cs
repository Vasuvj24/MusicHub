using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicHub.API.Extensions;
using MusicHub.Application.DTO;
using MusicHub.Application.DTO.Admin;
using MusicHub.Application.Services;

namespace MusicHub.API.Controllers
{
    [ApiController]
    [Route("api/posts")]
    public class PostController : ControllerBase
    {
        private readonly PostService _posts;
        private readonly AdminService _admin; 
        public PostController(PostService posts,AdminService admin)
        {
            _posts = posts;
            _admin = admin;
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
        [HttpGet("paged")]
        public async Task<IActionResult> GetPaged(
    [FromQuery] PostQueryDto dto,
    CancellationToken ct = default)
        {
            var result =
                await _posts.GetPagedAsync(dto, ct);

            return Ok(result);
        }
        [Authorize]
        [HttpDelete("{postId:guid}/like")]
        public async Task<IActionResult> Unlike(Guid postId,CancellationToken ct)
        {
            var userId = User.GetUserId();

            await _posts.UnlikeAsync(
                userId,
                postId,
                ct);

            return NoContent();
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
        [Authorize]
        [HttpPost("{postId:guid}/report")]
        public async Task<IActionResult> Report(Guid postId, [FromBody] ReportPostDto dto, CancellationToken ct)
        {
            var userId = User.GetUserId();
            await _admin.ReportPostAsync(userId, postId, dto.Reason, dto.Note, ct);
            return Ok();
        }
        [Authorize]
        [HttpDelete("{postId:guid}")]
        public async Task<IActionResult>Delete(Guid postId,CancellationToken ct)
        {
            var userId = User.GetUserId();

            await _posts.DeleteAsync(
                userId,
                postId,
                ct);

            return NoContent();
        }
        [HttpGet("search")]
        public async Task<IActionResult> Search(
    [FromQuery] string term)
        {
            return Ok(
                await _posts.SearchPostsAsync(term));
        }

        [HttpGet("{postId:guid}/comments")]
        public async Task<IActionResult> GetComments(
    Guid postId,
    CancellationToken ct)
        {
            return Ok(
                await _posts.GetCommentsAsync(postId, ct));
        }
        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> Get(
    Guid postId,
    CancellationToken ct)
        {
            return Ok(
                await _posts.GetByIdAsync(postId, ct));
        }
        [Authorize]
        [HttpDelete("{postId:guid}/comments/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(
    Guid postId,
    Guid commentId,
    CancellationToken ct)
        {
            var userId = User.GetUserId();

            await _posts.DeleteCommentAsync(
                userId,
                postId,
                commentId,
                ct);

            return NoContent();
        }
    }
}
