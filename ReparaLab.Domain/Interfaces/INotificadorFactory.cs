namespace ReparaLab.Domain.Interfaces;

public interface INotificadorFactory
{
    INotificador Crear(string tipo);
}