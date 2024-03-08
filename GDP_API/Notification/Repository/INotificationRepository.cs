using GDP_API.Data;

namespace GDP_API.Notification.Repository
{
    public interface INotificationRepository
    {
        Task AddAsync(NotificationEntity notification);
        Task SaveChangesAsync();
    }
}
namespace GDP_API.Notification.Repository
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly DataContext _context;

        public NotificationRepository(DataContext context)
        {
            _context = context;
        }

        public async Task AddAsync(NotificationEntity notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
