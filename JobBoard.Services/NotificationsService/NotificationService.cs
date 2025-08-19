using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Domain.Services.Contract;
using JobBoard.Repositories.Specifications;
using Microsoft.AspNetCore.SignalR;

namespace JobBoard.Services.NotificationsService
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly INotificationSender _notificationSender;

        public NotificationService(IUnitOfWork unitOfWork , IMapper mapper , INotificationSender notificationSender )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _notificationSender = notificationSender;
        }
        public async Task AddNotificationAsync(string userId, string message, string? link = null)
        {

            // This would typically involve saving the notification to a database
            var notification = new Notification
            {
                UserId = userId,
                Message = message,
                Link = link
            };
            var repository = _unitOfWork.Repository<Notification>();
            await repository.AddAsync(notification);
            await _unitOfWork.CompleteAsync();

            // Send the notification to the user via SignalR
            await _notificationSender.SendNotificationAsync(userId, message, link);
        }
        public async Task MarkAsReadAsync(int notificationId)
        {

            // This would typically involve updating the notification in the database
            var repository = _unitOfWork.Repository<Notification>();
            var notification = await repository.GetByIdAsync(notificationId);
            if (notification == null)
            {
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");
            }
            notification.IsRead = true;
            repository.Update(notification);
            await _unitOfWork.CompleteAsync();


            // Optionally, you could also send a SignalR message to update the UI
            await _notificationSender.SendNotificationAsync(notification.UserId, "Notification marked as read.");

        }
        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string userId)
        {
            var specification = new NotificationsForUserSpecification(userId);
            var repository = _unitOfWork.Repository<Notification>();
            var notifications = await repository.GetAllAsync(specification);
            if (notifications == null || !notifications.Any())
            {
                return Enumerable.Empty<NotificationDto>();
            }
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }
    }
}
