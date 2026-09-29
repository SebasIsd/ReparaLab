namespace ReparaLab.Infrastructure.Logging;
using ReparaLab.Domain.Interfaces;

public class LoggerGlobal : ILogger
{
    private static LoggerGlobal? _instancia;
    private static readonly object _lock = new();
    private readonly List<string> _eventos = new();

    private LoggerGlobal() { }

    public static LoggerGlobal Instancia
    {
        get
        {
            if (_instancia == null)
            {
                lock (_lock)
                {
                    _instancia ??= new LoggerGlobal();
                }
            }
            return _instancia;
        }
    }

    public void Registrar(string evento)
    {
        _eventos.Add($"{DateTime.Now:HH:mm:ss} | {evento}");
    }

    public List<string> ObtenerEventos()
    {
        return _eventos;
    }
}