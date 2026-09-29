namespace ReparaLab.Domain.Interfaces;
using ReparaLab.Domain.Entities;

public interface IOrdenCommandRepository
{
    Task Guardar(OrdenReparacion orden);
}