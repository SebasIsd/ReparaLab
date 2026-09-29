namespace ReparaLab.Infrastructure.Notifications;
using ReparaLab.Domain.Interfaces;

public class NotificadorSms : INotificador
{
    public void Enviar(string mensaje)
    {
        Console.WriteLine($"Enviando SMS: {mensaje}");
    }
}