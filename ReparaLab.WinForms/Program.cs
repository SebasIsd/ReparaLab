using Microsoft.Extensions.DependencyInjection;
using ReparaLab.Application.UseCases;
using ReparaLab.Domain.Interfaces;
using ReparaLab.Infrastructure.Logging;
using ReparaLab.Infrastructure.Notifications;
using ReparaLab.Infrastructure.Persistence;
using ReparaLab.Infrastructure.Plans;

namespace ReparaLab.WinForms;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        var services = new ServiceCollection();

        // Infraestructura (Singleton para memoria)
        services.AddSingleton<RepositorioEnMemoria>();
        services.AddSingleton<IOrdenCommandRepository>(sp => sp.GetRequiredService<RepositorioEnMemoria>());
        services.AddSingleton<IOrdenQueryRepository>(sp => sp.GetRequiredService<RepositorioEnMemoria>());

        // Singleton para el Logger
        services.AddSingleton<ILogger>(LoggerGlobal.Instancia);

        // Fábricas y Proveedores (Singleton o Transient, da igual, no guardan estado)
        services.AddSingleton<INotificadorFactory, NotificadorFactory>();
        services.AddSingleton<IPlanFactoryProvider, PlanFactoryProvider>();
        // Casos de uso
        services.AddTransient<RegistrarOrdenUseCase>();
        services.AddTransient<ListarOrdenesUseCase>();
        services.AddTransient<CambiarEstadoOrdenUseCase>();

        // UI
        services.AddTransient<FrmReparaciones>();

        var provider = services.BuildServiceProvider();
        System.Windows.Forms.Application.Run(provider.GetRequiredService<FrmReparaciones>());
    }
}