using Microsoft.Extensions.DependencyInjection;
using TesteDevAuvo;

class Program
{
  static async Task Main(string[] args)
  {
    Escopo escopo = Container.ServiceProvider.GetRequiredService<Escopo>();
    await escopo.Executar();
  }
}