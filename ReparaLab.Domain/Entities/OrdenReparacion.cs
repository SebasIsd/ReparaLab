namespace ReparaLab.Domain.Entities;

public class OrdenReparacion
{
    public int Id { get; private set; }
    public string Cliente { get; private set; } = "";
    public string Equipo { get; private set; } = "";
    public string Falla { get; private set; } = "";
    public string Servicio { get; private set; } = "";
    public string Plan { get; private set; } = "";
    public string Notificacion { get; private set; } = "";
    public bool Repuesto { get; private set; }
    public decimal Total { get; private set; }
    public int GarantiaDias { get; private set; }
    public string Estado { get; private set; } = "PENDIENTE";

    // Constructor público para ser usado por el Builder
    public OrdenReparacion(
        int id, 
        string cliente, 
        string equipo, 
        string falla, 
        string servicio,
        string plan, 
        string notificacion, 
        bool repuesto, 
        decimal total, 
        int garantiaDias)
    {
        Id = id;
        Cliente = cliente;
        Equipo = equipo;
        Falla = falla;
        Servicio = servicio;
        Plan = plan;
        Notificacion = notificacion;
        Repuesto = repuesto;
        Total = total;
        GarantiaDias = garantiaDias;
    }

    public void Finalizar()
    {
        if (Estado != "PENDIENTE")
            throw new InvalidOperationException("Solo se puede finalizar una orden PENDIENTE.");
        Estado = "FINALIZADA";
    }

    public void Cancelar()
    {
        if (Estado != "PENDIENTE")
            throw new InvalidOperationException("Solo se puede cancelar una orden PENDIENTE.");
        Estado = "CANCELADA";
    }
}