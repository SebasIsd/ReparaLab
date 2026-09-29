namespace ReparaLab.Domain.Interfaces;
using ReparaLab.Domain.Entities;

public interface IOrdenQueryRepository
{
    Task<List<OrdenReparacion>> ObtenerTodas();
    Task<OrdenReparacion?> ObtenerPorId(int id);
}