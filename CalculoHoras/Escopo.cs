using Newtonsoft.Json;
using TesteDevAuvo.Dominio;
using TesteDevAuvo.Services;

namespace TesteDevAuvo;
public class Escopo
{
  private IFileService _fileService { get; }

  public Escopo(IFileService fileService)
  {
    _fileService = fileService;
  }

  public async Task Executar()
  {
    bool continuar = true;
    string caminhoPasta;
    while (continuar)
    {
      Console.Write("Digite o caminho da pasta com os arquivos CSV: ");
      caminhoPasta = Console.ReadLine() ?? string.Empty;

      if (!Directory.Exists(caminhoPasta))
      {
        Console.WriteLine($"O diretório {caminhoPasta} não foi encontrado.");
        continue;
      }

      List<RegistroPagamento> departamentos = await _fileService.ReadFilesAsync(caminhoPasta);

      string json = JsonConvert.SerializeObject(departamentos, Formatting.Indented);
      string saida = Path.Combine(caminhoPasta, "saida.json");
      File.WriteAllText(saida, json);
      Console.WriteLine($"Arquivo JSON gravado em {saida}");

      Console.Write("Deseja processar outra pasta? (S/N) ");
      string resposta = Console.ReadLine() ?? string.Empty; ;
      continuar = resposta.Equals("S", StringComparison.OrdinalIgnoreCase);
    }
  }
}