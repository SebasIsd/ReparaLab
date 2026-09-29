namespace ReparaLab.Domain.Interfaces;

public interface ILogger
{
    void Registrar(string evento);
    List<string> ObtenerEventos();
}