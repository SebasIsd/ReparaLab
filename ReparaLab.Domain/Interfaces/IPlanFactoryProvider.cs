namespace ReparaLab.Domain.Interfaces;

public interface IPlanFactoryProvider
{
    IPlanFactory ObtenerFabrica(string plan);
}