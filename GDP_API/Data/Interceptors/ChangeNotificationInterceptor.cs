using GDP_API.Notification.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public class ChangeNotificationInterceptor : SaveChangesInterceptor
{
    private readonly INotificationService _notificationService;

    public ChangeNotificationInterceptor(INotificationService notificationService)
    {
        _notificationService = notificationService;
    }

    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        var context = eventData.Context;
        var entries = context.ChangeTracker.Entries();

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified || entry.State == EntityState.Deleted)
            {
                // Aquí puedes determinar el tipo de entidad y generar la notificación correspondiente
                var entityType = entry.Entity.GetType().Name;
                var changeType = entry.State.ToString();

                // método para crear notificaciones genéricas
                _notificationService.CreateGenericChangeNotification(entityType, changeType);
            }
        }

        return base.SavingChanges(eventData, result);
    }
}
