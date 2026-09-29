namespace ReparaLab.Infrastructure.Persistence;
using ReparaLab.Domain.Entities;
using ReparaLab.Domain.Interfaces;

public class RepositorioEnMemoria : IOrdenCommandRepository, IOrdenQueryRepository
{
    private readonly List<OrdenReparacion> _db = new();

    public Task Guardar(OrdenReparacion orden)
    {
        _db.Add(orden);
        return Task.CompletedTask;
    }

    public Task<List<OrdenReparacion>> ObtenerTodas()
    {
        return Task.FromResult(_db.ToList());
    }

    public Task<OrdenReparacion?> ObtenerPorId(int id)
    {
        return Task.FromResult(_db.FirstOrDefault(x => x.Id == id));
    }
}