namespace GDP_API.Notification.Interfaces
{
    public interface INotificationService
    {
        Task CreateGenericChangeNotification(string entityType, string changeType);
        // Agregar más métodos según se necesite.
    }
}
