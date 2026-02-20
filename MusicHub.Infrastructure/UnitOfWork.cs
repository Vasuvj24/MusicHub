using MusicHub.Application.Interfaces;
using MusicHub.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure
{

    public class UnitOfWork : IUnitOfWork
    {
        //used to perform th operations on db 
        private readonly AppDbContext _context;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
            Console.WriteLine("UoW ctx: " + _context.GetHashCode());

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
