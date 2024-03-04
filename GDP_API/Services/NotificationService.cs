using GDP_API.Data;



public interface INotificationService
{
    Task CreateGenericChangeNotification(string entityType, string changeType);
    // Agregar más métodos según se necesite.
}

public class NotificationService : INotificationService
{
    private readonly DataContext _context;

    public NotificationService(DataContext context)
    {
        _context = context;
    }

    public async Task CreateGenericChangeNotification(string entityType, string changeType)
    {
        // Crear y almacenar una notificación en la base de datos
        var notification = new Notification
        {
            // propiedades de la notificación
            Title = $"Cambio detectado en {entityType}",
            Message = $"Se ha realizado una operación de tipo {changeType} en la entidad {entityType}.",
            CreatedAt = DateTime.UtcNow,
            IsRead = false,
            UserId = "System" // Reemplaza con el ID de usuario apropiado.
        };

        _context.Notifications.Add(notification);
        await _context.SaveChangesAsync();
    }
}
