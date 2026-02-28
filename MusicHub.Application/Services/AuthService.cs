using MusicHub.Application.DTO;
using MusicHub.Application.DTO.Auth;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Domain.Users;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace MusicHub.Application.Services
{
            public sealed class AuthService
            {
                private readonly IUserRepository _users;
                private readonly IRefreshTokenRepository _refreshTokens;
                private readonly ITokenService _tokenService;
                private readonly IUnitOfWork _uow;
                private readonly IClock _clock;

                public AuthService(
                    IUserRepository users,
                    IRefreshTokenRepository refreshTokens,
                    ITokenService tokenService,
                    IUnitOfWork uow,
                    IClock clock)
                {
                    _users = users;
                    _refreshTokens = refreshTokens;
                    _tokenService = tokenService;
                    _uow = uow;
                    _clock = clock;
                }

                public async Task RegisterAsync(RegisterUserDto dto, CancellationToken ct)
                {
                    var existing = await _users.GetByEmailAsync(dto.Email);
                    if (existing != null) throw new InvalidOperationException("User already exists.");

                    var hash = BCrypt.Net.BCrypt.HashPassword(dto.Password);
                    var user = new User(dto.Email, hash, Role.User);

                    await _users.AddAsync(user);
                    await _uow.SaveChangesAsync();
                }

                public async Task<AuthResDto> LoginAsync(LoginDto dto, CancellationToken ct)
                {
                    var user = await _users.GetByEmailAsync(dto.Email)
                               ?? throw new UnauthorizedAccessException("Invalid credentials.");

                    if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                        throw new UnauthorizedAccessException("Invalid credentials.");

                    var access = _tokenService.CreateAccessToken(user);

                    var refreshPlain = GenerateSecureToken();
                    var refreshHash = Sha256(refreshPlain);

                    var now = _clock.UtcNow;
                    var refresh = new RefreshToken(
                        userId: user.Id,
                        tokenHash: refreshHash,
                        createdAtUtc: now,
                        expiresAtUtc: now.AddDays(14)
                    );

                    await _refreshTokens.AddAsync(refresh, ct);
                    await _uow.SaveChangesAsync();

                    return new AuthResDto
                    {
                        AccessToken = access,
                        RefreshToken = refreshPlain
                    };
                }

                public async Task<AuthResDto> RefreshAsync(RefreshRequestDto dto, CancellationToken ct)
                {
                    if (string.IsNullOrWhiteSpace(dto.RefreshToken))
                        throw new ArgumentException("RefreshToken is required.");

                    var oldHash = Sha256(dto.RefreshToken);
                    var old = await _refreshTokens.GetActiveByHashAsync(oldHash, ct)
                              ?? throw new UnauthorizedAccessException("Invalid refresh token.");
                    //checkin if that user exist for that hash or not
                    var user = await _users.GetByIdAsync(old.UserId)
                               ?? throw new UnauthorizedAccessException("User not found.");

                    // rotate token
                    var newPlain = GenerateSecureToken();
                    var newHash = Sha256(newPlain);

                    var now = _clock.UtcNow;

                    var newRt = new RefreshToken(user.Id, newHash, now, now.AddDays(14));
                    await _refreshTokens.AddAsync(newRt, ct);

                    old.Revoke(now, newRt.Id);

                    await _uow.SaveChangesAsync();

                    return new AuthResDto
                    {
                        AccessToken = _tokenService.CreateAccessToken(user),
                        RefreshToken = newPlain
                    };
                }

                public async Task LogoutAsync(Guid userId, CancellationToken ct)
                {
                    //revoke all the refresh token to avoid it getting utilised
                    var tokens = await _refreshTokens.GetActiveByUserAsync(userId, ct);
                    var now = _clock.UtcNow;

                    foreach (var t in tokens)
                        t.Revoke(now);

                    await _uow.SaveChangesAsync();
                }

                private static string GenerateSecureToken()
                {
                    var bytes = RandomNumberGenerator.GetBytes(64);
                    return Convert.ToBase64String(bytes);
                }

                private static string Sha256(string input)
                {
                    using var sha = SHA256.Create();
                    var bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(input));
                    return Convert.ToHexString(bytes); // uppercase hex
                }
            }
}
