using MusicHub.Application.DTO;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public sealed class PostService
    {
        private readonly IPostRepository _posts;
        private readonly IUnitOfWork _uow;
        public PostService(IPostRepository posts, IUnitOfWork uow)
        {
            _posts = posts;
            _uow = uow;
        }
        public async Task LikeAsync(Guid currentUserId, Guid postId, LikePostDto dto, CancellationToken ct)
        {
            var post = await _posts.GetByIdAsync(postId, ct)
                       ?? throw new KeyNotFoundException("Post not found.");

            post.Like(currentUserId);

            await _uow.SaveChangesAsync();
        }
        public async Task<Guid> CreateAsync(Guid currentUserId,CreatePostDto dto, CancellationToken ct)
        {
            var post = new Post(currentUserId, dto.Instrument, dto.MediaUrl, dto.Caption ?? "");
            await _posts.AddAsync(post, ct);
            await _uow.SaveChangesAsync(); // your existing UoW
            return post.Id;
        }
        public async Task CommentAsync(Guid currentUserId, Guid postId, AddCommentDto dto, CancellationToken ct)
        {
            var post = await _posts.GetByIdAsync(postId, ct)
                       ?? throw new KeyNotFoundException("Post not found.");

            post.AddComment(currentUserId, dto.Text);

            await _uow.SaveChangesAsync();
        }
        public async Task<List<PostResponseDto>> GetLatestAsync(int take, CancellationToken ct)
        {
            take = Math.Clamp(take, 1, 50);
            var posts = await _posts.GetLatestAsync(take, ct);
            //returning dto and mapping data from entity
            return posts.Select(p => new PostResponseDto
            {
                Id = p.Id,
                UserId = p.UserId,
                Instrument = p.Instrument,
                MediaUrl = p.MediaUrl,
                Caption = p.Caption,
                CreatedAtUtc = p.CreatedAtUtc,
                LikesCount = p.Likes.Count,
                CommentsCount = p.Comments.Count
            }).ToList();
        }
    }
}
