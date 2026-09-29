namespace ReparaLab.Infrastructure.Notifications;
using ReparaLab.Domain.Interfaces;

public class NotificadorEmail : INotificador
{
    public void Enviar(string mensaje)
    {
        Console.WriteLine($"Enviando correo electrónico con el mensaje: {mensaje}");
    }
}