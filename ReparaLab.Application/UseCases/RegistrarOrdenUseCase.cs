namespace ReparaLab.Application.UseCases;
using ReparaLab.Domain.Builders;
using ReparaLab.Domain.Entities;
using ReparaLab.Domain.Interfaces;
using System.Numerics;

public class RegistrarOrdenUseCase
{
    private readonly IOrdenCommandRepository _commandRepo;
    private readonly INotificadorFactory _notificadorFactory;
    private readonly ILogger _logger;
    private readonly IPlanFactoryProvider _planProvider;
    private int _siguienteId = 1;

    public RegistrarOrdenUseCase(IOrdenCommandRepository commandRepo, INotificadorFactory notificadorFactory, ILogger logger, IPlanFactoryProvider planProvider)
    {
        _commandRepo = commandRepo;
        _notificadorFactory = notificadorFactory;
        _logger = logger;
        _planProvider = planProvider;
    }

    public async Task Ejecutar(
        string cliente, 
        string equipo, 
        string falla, 
        string servicio, 
        string plan, 
        string notificacion, 
        bool repuesto)
    {
        //obtener el Abstract Factory a traves del Provider (Sin el uso de new de Infrastructure)
        IPlanFactory fabricaPlan = _planProvider.ObtenerFabrica(plan);

        //uso del Builder para construir la orden paso a paso
        var orden = new OrdenBuilder()
            .SetId(_siguienteId++)
            .ConCliente(cliente)
            .ConEquipo(equipo)
            .ConFalla(falla)
            .ConServicio(servicio)
            .ConPlan(plan)
            .ConNotificacion(notificacion)
            .ConRepuesto(repuesto)
            .CalcularPrecioYGarantia(fabricaPlan)
            .Construir();

        await _commandRepo.Guardar(orden);

        //notificar usando Factory Method
        var notificador = _notificadorFactory.Crear(notificacion);
        notificador.Enviar($"Orden {orden.Id} registrada para {cliente}");
        //uso de Singleton para loguear la creacion de la orden
        _logger.Registrar($"Orden {orden.Id} creada. Total: ${orden.Total:0.00}");
    }
}