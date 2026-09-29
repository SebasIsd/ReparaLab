namespace ReparaLab.Application.UseCases;
using ReparaLab.Domain.Interfaces;

public class CambiarEstadoOrdenUseCase
{
    private readonly IOrdenQueryRepository _queryRepo;
    private readonly ILogger _logger;

    public CambiarEstadoOrdenUseCase(IOrdenQueryRepository queryRepo, ILogger logger)
    {
        _queryRepo = queryRepo;
        _logger = logger;
    }

    public async Task Ejecutar(int id, string accion)
    {
        var orden = await _queryRepo.ObtenerPorId(id);
        if (orden == null) throw new Exception("Orden no encontrada.");

        if (accion == "FINALIZAR") orden.Finalizar();
        else if (accion == "CANCELAR") orden.Cancelar();
        else throw new ArgumentException("Acción no válida");

        _logger.Registrar($"Orden {id} cambió a {orden.Estado}");
    }
}