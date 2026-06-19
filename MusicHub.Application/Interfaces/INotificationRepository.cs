using MusicHub.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MusicHub.Application.Interfaces
{
    public interface INotificationRepository
    {
        Task AddAsync(Notification notification);

        Task<List<Notification>>
            GetUserNotificationsAsync(Guid userId);

        Task<Notification?>
            GetByIdAsync(Guid id);

        Task<int>
            GetUnreadCountAsync(Guid userId);
    }
}
