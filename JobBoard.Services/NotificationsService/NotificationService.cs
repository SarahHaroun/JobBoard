using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using JobBoard.Domain.DTO.NotificationsDto;
using JobBoard.Domain.Entities;
using JobBoard.Domain.Repositories.Contract;
using JobBoard.Repositories.Specifications;

namespace JobBoard.Services.NotificationsService
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public NotificationService(IUnitOfWork unitOfWork , IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
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
