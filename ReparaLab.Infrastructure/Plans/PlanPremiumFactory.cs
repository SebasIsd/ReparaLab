namespace ReparaLab.Infrastructure.Plans;
using ReparaLab.Domain.Interfaces;

public class PlanPremiumFactory : IPlanFactory
{
    public decimal ObtenerTarifaBase(string servicio) => servicio == "DIAGNOSTICO" ? 20m : 35m;
    public int ObtenerGarantia() => 90;
    public bool EsPremium() => true;
}