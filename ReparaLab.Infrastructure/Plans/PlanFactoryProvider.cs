namespace ReparaLab.Infrastructure.Plans;
using ReparaLab.Domain.Interfaces;

public class PlanFactoryProvider : IPlanFactoryProvider
{
    public IPlanFactory ObtenerFabrica(string plan)
    {
        return plan switch
        {
            "PREMIUM" => new PlanPremiumFactory(),
            "BASICO" => new PlanBasicoFactory(),
            _ => throw new ArgumentException("Plan no válido")
        };
    }
}