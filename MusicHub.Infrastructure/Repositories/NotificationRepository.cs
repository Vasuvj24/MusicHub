using MusicHub.Application.Interfaces;
using MusicHub.Domain.Entities;
using MusicHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Infrastructure.Repositories
{
    public class NotificationRepository
    : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(
            AppDbContext context)
        {
            _context = context;
        }
        public Task MarkAsReadAsync(Notification notification)
        {
            notification.MarkRead();

            return Task.CompletedTask;
        }
        public async Task AddAsync(Notification notification)
        {
            await _context.Notifications
                .AddAsync(notification);
        }

        public async Task<List<Notification>> GetUserNotificationsAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAtUtc)
                .ToListAsync();
        }

        public async Task<Notification?> GetByIdAsync(Guid id)
        {
            return await _context.Notifications
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _context.Notifications
                .CountAsync(x =>
                    x.UserId == userId &&
                    !x.IsRead);
        }
    }
}
