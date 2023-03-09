using Microsoft.Extensions.DependencyInjection;
using TesteDevAuvo.Services;
using TesteDevAuvo.Services.Impl;

namespace TesteDevAuvo;
public class Container
{
  private static ServiceProvider? serviceProvider;
  public static ServiceProvider ServiceProvider
  {
    get
    {
      serviceProvider ??= Configurar();
      return serviceProvider;
    }
  }

  private static ServiceProvider Configurar()
  {
    return new ServiceCollection()
        .AddSingleton<IRegistroPagamentoService, RegistroPagamentoService>()
        .AddSingleton<IFileService, FileService>()
        .AddSingleton<Escopo>()
        .BuildServiceProvider();
  }
}
