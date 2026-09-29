namespace ReparaLab.Infrastructure.Notifications;
using ReparaLab.Domain.Interfaces;

public class NotificadorFactory : INotificadorFactory
{
    public INotificador Crear(string tipo)
    {
        return tipo switch
        {
            "EMAIL" => new NotificadorEmail(),
            "SMS" => new NotificadorSms(),
            _ => throw new ArgumentException("Tipo de notificación no válido")
        };
    }
}