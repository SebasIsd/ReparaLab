namespace ReparaLab.Domain.Interfaces;

public interface IPlanFactory
{
    decimal ObtenerTarifaBase(string servicio);
    int ObtenerGarantia();
    bool EsPremium();
}