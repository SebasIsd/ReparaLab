namespace ReparaLab.Infrastructure.Plans;
using ReparaLab.Domain.Interfaces;

public class PlanBasicoFactory : IPlanFactory
{
    public decimal ObtenerTarifaBase(string servicio) => servicio == "DIAGNOSTICO" ? 20m : 35m;
    public int ObtenerGarantia() => 30;
    public bool EsPremium() => false;
}