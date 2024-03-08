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
        // Crear y almacenar una notificaci�n a trav�s del repositorio
        var notification = new NotificationEntity
        {
            Title = $"Cambio detectado en {entityType}",
            Message = $"Se ha realizado una operaci�n de tipo {changeType} en la entidad {entityType}.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            UserId = "System" // Aseg�rate de tener una l�gica para determinar el UserId
        };

        await _notificationRepository.AddAsync(notification);
        await _notificationRepository.SaveChangesAsync();
    }
}
