using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Domain.Entities
{
    public sealed class RefreshToken
    {
        public Guid Id { get; private set; } = Guid.NewGuid();

        public Guid UserId { get; private set; }
        public string TokenHash { get; private set; } = string.Empty;

        public DateTime CreatedAtUtc { get; private set; }
        public DateTime ExpiresAtUtc { get; private set; }

        public DateTime? RevokedAtUtc { get; private set; }
        public Guid? ReplacedByTokenId { get; private set; }

        public bool IsActive => RevokedAtUtc == null && DateTime.UtcNow < ExpiresAtUtc;

        private RefreshToken() { } // EF for ef to push because it creates the object and then pushes the values
        public RefreshToken(Guid userId, string tokenHash, DateTime createdAtUtc, DateTime expiresAtUtc)
        {
            UserId = userId;
            TokenHash = tokenHash;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }
        public void Revoke(DateTime revokedAtUtc, Guid? replacedByTokenId = null)
        {
            RevokedAtUtc = revokedAtUtc;
            ReplacedByTokenId = replacedByTokenId;
        }
    }
}
