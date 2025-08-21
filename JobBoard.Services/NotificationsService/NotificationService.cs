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

        /////////////////////////////// Add Notification //////////////////////////////////////////
        public async Task AddNotificationAsync(string userId, string message, string? link = null)
        {
            try
            {
                var notification = new Notification
                {
                    UserId = userId,
                    Message = message,
                    Link = link
                };
                var repository = _unitOfWork.Repository<Notification>();
                await repository.AddAsync(notification);
                await _unitOfWork.CompleteAsync();
                await _notificationSender.SendNotificationAsync(userId, message, link, notification.Id); // أضيفي Id
            }
            catch (Exception ex)
            {
                throw new Exception($"Error adding notification: {ex.Message}", ex);
            }
        }


        /////////////////////////////// Mark As Read //////////////////////////////////////////
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


            await _notificationSender.SendNotificationUpdateAsync(notification.UserId, new
            {
                Action = "MarkAsRead",
                NotificationId = notificationId,
                IsRead = true
            });

        }


        /////////////////////////////// Get User Notification //////////////////////////////////////////
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


        /////////////////////////////// Mark All As Read //////////////////////////////////////////

        public async Task MarkAllAsReadAsync(string userId)
        {
            var specification = new NotificationsForUserSpecification(userId);
            var repository = _unitOfWork.Repository<Notification>();
            var notifications = await repository.GetAllAsync(specification);

            foreach (var notification in notifications.Where(n => !n.IsRead))
            {
                notification.IsRead = true;
                repository.Update(notification);
            }

            await _unitOfWork.CompleteAsync();
            if (notifications.Any())
            {
                await _notificationSender.SendNotificationUpdateAsync(userId, new
                {
                    Action = "MarkAllAsRead",
                    NotificationIds = notifications
                });
            }
        }

        /////////////////////////////// Delete Notification //////////////////////////////////////////

        public async Task DeleteNotificationAsync(int notificationId)
        {
            var repository = _unitOfWork.Repository<Notification>();
            var notification = await repository.GetByIdAsync(notificationId);

            if (notification == null)
                throw new KeyNotFoundException($"Notification with ID {notificationId} not found.");

            repository.Delete(notification);
            await _unitOfWork.CompleteAsync();

            await _notificationSender.SendNotificationUpdateAsync(notification.UserId, new
            {
                Action = "Delete",
                NotificationId = notificationId
            });
        }
    }
}
