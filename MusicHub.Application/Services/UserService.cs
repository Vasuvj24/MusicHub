using Microsoft.EntityFrameworkCore.Migrations.Operations;
using MusicHub.Application.DTO;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public class UserService
    {
        private readonly IUserRepository _repo;
        private readonly IUnitOfWork _uow;
        private readonly ITokenService tokens;
        public UserService(IUserRepository repo, IUnitOfWork uow,ITokenService _tokens)
        {
            _repo = repo;
            _uow = uow;
            tokens = _tokens;
        }
        //used for registering the 
        public async Task RegisterAsync(string email,string password)
        {
            var existing = await _repo.GetByEmailAsync(email);
            if (existing != null)
                throw new Exception("user already exist");
            var hash = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User(email, hash, Role.User);
            await _repo.AddAsync(user);
            await _uow.SaveChangesAsync();
        }
        //giving new token whule logging in
        public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
        {
            var user = await _repo.GetByEmailAsync(dto.Email) ?? throw new UnauthorizedAccessException("invalid credentials");
            var ok = BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash);
            if (!ok) throw new UnauthorizedAccessException("invalid credentials");
            return new AuthResponseDto { AccessToken = tokens.CreateAccessToken(user) };
        }
    }
}
