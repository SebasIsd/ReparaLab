namespace ReparaLab.Domain.Builders;
using ReparaLab.Domain.Entities;
using ReparaLab.Domain.Interfaces;

public class OrdenBuilder
{
    private int _id;
    private string _cliente = "";
    private string _equipo = "";
    private string _falla = "";
    private string _servicio = "";
    private string _plan = "";
    private string _notificacion = "";
    private bool _repuesto;
    private decimal _total;
    private int _garantiaDias;

    public OrdenBuilder SetId(int id) { _id = id; return this; }

    public OrdenBuilder ConId(int id)
    {
        _id = id;
        return this;
    }
    public OrdenBuilder ConCliente(string cliente)
    {
        _cliente = cliente;
        return this;
    }
    public OrdenBuilder ConEquipo(string equipo)
    {
        _equipo = equipo;
        return this;
    }
    public OrdenBuilder ConFalla(string falla)
    {
        _falla = falla;
        return this;
    }  
    public OrdenBuilder ConServicio(string servicio)
    {
        _servicio = servicio;
        return this;
    }
    public OrdenBuilder ConPlan(string plan)
    {
        _plan = plan;
        return this;
    }
    public OrdenBuilder ConNotificacion(string notificacion)
    {
        _notificacion = notificacion;
        return this;
    }
    public OrdenBuilder ConRepuesto(bool repuesto)
    {
        _repuesto = repuesto;
        return this;
    }

    public OrdenBuilder CalcularPrecioYGarantia(IPlanFactory fabricaPlan)
    {
        _total = fabricaPlan.ObtenerTarifaBase(_servicio);
        if (fabricaPlan.EsPremium()) _total *= 1.25m;
        if (_repuesto) _total += 15m;

        _garantiaDias = fabricaPlan.ObtenerGarantia();
        return this;
    }

    public OrdenReparacion Construir()
    {
        return new OrdenReparacion(
            _id, 
            _cliente, 
            _equipo, 
            _falla, 
            _servicio, 
            _plan, 
            _notificacion, 
            _repuesto,
            _total, 
            _garantiaDias);
    }
}