using MusicHub.Domain.Common;
using MusicHub.Domain.Enums;
using MusicHub.Domain.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public class Post : BaseEntity
    {
        //we made postlike and postcomments in order to not expose this outside and make it update from outside we made post as the root aggregate for both rather then using naviagtional property we used this
        //backing field for likes and posts
        private readonly List<PostLike> _likes = new();
        private readonly List<PostComment> _comments = new();
        public Guid UserId { get; private set;  }
        public InstrumentType Instrument { get; private set; }
        public string Caption { get; private set; } = string.Empty;

        // For Phase 2 we store only a URL/path (actual upload later in Phase 2.5/3)
        public string MediaUrl { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; } = DateTime.UtcNow;
        //ireadonly because it doesn't allow add or remove like readonly 
        public IReadOnlyCollection<PostLike> Likes => _likes;
        public IReadOnlyCollection<PostComment> Comments => _comments;
        public Post(Guid userId, InstrumentType instrument, string mediaUrl, string caption)
        {
            if (userId == Guid.Empty) throw new ArgumentNullException("user id is null");
            if (string.IsNullOrWhiteSpace(mediaUrl)) throw new ArgumentNullException("media url is null");
            UserId = userId;
            Instrument = instrument;
            MediaUrl = mediaUrl.Trim();
            Caption = caption?.Trim() ?? string.Empty;
            AddDomainEvent(new PostCreatedEvent(Id, UserId));
        }
        public void Like(Guid userId)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Invalid userId");
            if (_likes.Any(x => x.UserId == userId))
                throw new InvalidOperationException("Already liked.");

            _likes.Add(new PostLike(Id, userId));
            AddDomainEvent(new PostLikedEvent(Id, userId));
        }
        public void AddComment(Guid userId, string text)
        {
            if (userId == Guid.Empty) throw new ArgumentException("Invalid userId");
            if (string.IsNullOrWhiteSpace(text))
                throw new InvalidOperationException("Comment cannot be empty.");

            _comments.Add(new PostComment(Id, userId, text.Trim()));
            AddDomainEvent(new PostCommentEvent(Id, userId));
        }
    }
}
