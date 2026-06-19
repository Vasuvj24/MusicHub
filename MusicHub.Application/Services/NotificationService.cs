using MusicHub.Application.DTO;
using MusicHub.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Services
{
    public class NotificationService
    {
        private readonly INotificationRepository _repo;
        private readonly IUnitOfWork _uow;

        public NotificationService(
            INotificationRepository repo,
            IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<List<NotificationResponseDto>>
            GetNotificationsAsync(Guid userId)
        {
            var notifications =
                await _repo.GetUserNotificationsAsync(userId);

            return notifications.Select(x =>
                new NotificationResponseDto
                {
                    Id = x.Id,
                    Message = x.Message,
                    IsRead = x.IsRead,
                    CreatedAtUtc = x.CreatedAtUtc
                }).ToList();
        }

        public async Task MarkAsReadAsync(Guid id)
        {
            var notification =
                await _repo.GetByIdAsync(id);

            if (notification == null)
                throw new Exception("Notification not found");

            notification.IsRead = true;

            await _uow.SaveChangesAsync();
        }

        public async Task<int>
            GetUnreadCountAsync(Guid userId)
        {
            return await _repo
                .GetUnreadCountAsync(userId);
        }
    }
}
