using Microsoft.EntityFrameworkCore;
using MusicHub.Application.Interfaces;
using MusicHub.Domain.Users;
using MusicHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository (AppDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(x => x.Email == email);
        }
    }
}
