using ReparaLab.Domain.Entities;
using ReparaLab.Domain.Interfaces;
using System.Numerics;

namespace ReparaLab.Domain.Prototypes;

public class PlantillaServicio : ICloneable
{
    public string Tipo { get; set; } = "";
    public List<string> Tareas { get; set; } = new();

    public object Clone()
    {
        return new PlantillaServicio
        {
            Tipo = this.Tipo,
            Tareas = new List<string>(this.Tareas) // Copia profunda
        };
    }
}
