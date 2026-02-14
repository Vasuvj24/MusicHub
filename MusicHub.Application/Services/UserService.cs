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
        public UserService(IUserRepository repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }
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
    }
}
