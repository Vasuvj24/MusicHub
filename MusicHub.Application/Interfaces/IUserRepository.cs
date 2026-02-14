using MusicHub.Domain.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface IUserRepository
    {
        Task AddAsync(User User);
        Task<User?> GetByEmailAsync(string Email);
    }
}
