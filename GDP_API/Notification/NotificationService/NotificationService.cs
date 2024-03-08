using GDP_API.Notification.Interfaces;
using GDP_API.Notification.Repository;


public class NotificationService : INotificationService
{
    private readonly INotificationRepository _notificationRepository;

    public NotificationService(INotificationRepository notificationRepository)
    {
        _notificationRepository = notificationRepository;
    }

    public async Task CreateGenericChangeNotification(string entityType, string changeType)
    {
        // Crear y almacenar una notificación a través del repositorio
        var notification = new NotificationEntity
        {
            Title = $"Cambio detectado en {entityType}",
            Message = $"Se ha realizado una operación de tipo {changeType} en la entidad {entityType}.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            UserId = "System" // Asegúrate de tener una lógica para determinar el UserId
        };

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }
}
