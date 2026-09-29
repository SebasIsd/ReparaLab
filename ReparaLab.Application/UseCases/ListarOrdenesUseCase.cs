namespace ReparaLab.Application.UseCases;
using ReparaLab.Domain.Entities;
using ReparaLab.Domain.Interfaces;

public class ListarOrdenesUseCase
{
    private readonly IOrdenQueryRepository _queryRepo;

    public ListarOrdenesUseCase(IOrdenQueryRepository queryRepo)
    {
        _queryRepo = queryRepo;
    }

    public async Task<List<OrdenReparacion>> Ejecutar()
    {
        return await _queryRepo.ObtenerTodas();
    }
}