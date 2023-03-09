using System.Globalization;
using TesteDevAuvo.Dominio;
using TesteDevAuvo.Extensions;
using TesteDevAuvo.ValueObjects;

namespace TesteDevAuvo.Services.Impl;
public class FileService : IFileService
{
  private const char Separator = ';';
  private readonly IRegistroPagamentoService _registroPagamentoService;

  public FileService(IRegistroPagamentoService registroPagamentoService)
  {
    _registroPagamentoService = registroPagamentoService;
  }

  public async Task<List<RegistroPagamento>> ReadFilesAsync(string caminhoPasta)
  {
    List<RegistroPagamento> departamentos = new();
    string[] arquivos = Directory.GetFiles(caminhoPasta, "*.csv");
    List<Task<RegistroPagamento>> tarefas = new();
    foreach (string arquivo in arquivos)
    {
      Console.WriteLine($"Processando arquivo {arquivo}...");
      tarefas.Add(ReadFileAsync(arquivo));
    }

    foreach (RegistroPagamento? tarefa in await Task.WhenAll(tarefas))
    {
      departamentos.Add(tarefa);
    }

    return departamentos;
  }

  public async Task<RegistroPagamento> ReadFileAsync(string caminho)
  {
    RegistroPagamento departamento = new();
    string fullFileName = Path.GetFileNameWithoutExtension(caminho);
    string[] dadosFile = fullFileName.Split('-');
    DateTime primeiroDiaMes, ultimoDiaMes;

    try
    {
      departamento.Departamento = dadosFile[0];
      departamento.MesVigencia = dadosFile[1];
      departamento.AnoVigencia = int.Parse(dadosFile[2]);
      primeiroDiaMes = DateTime.ParseExact(departamento.MesVigencia + " " + departamento.AnoVigencia, "MMMM yyyy", CultureInfo.GetCultureInfo("pt-BR"));
      ultimoDiaMes = primeiroDiaMes.AddMonths(1).AddSeconds(-1);
    }
    catch (Exception ex)
    {
      Console.WriteLine($"Erro no arquivo {caminho}, nome do arquivo em formato inválido: {ex.Message}");
      return departamento;
    }

    List<FuncionarioVO> registroFuncionarios = await ConverteArquivo(caminho, primeiroDiaMes, ultimoDiaMes);
    int quantidadeDiasUteis = primeiroDiaMes.GetDiasUteis();

    return _registroPagamentoService.ProcessaDadosEntrada(departamento, registroFuncionarios, quantidadeDiasUteis);
  }

  public async Task<List<FuncionarioVO>> ConverteArquivo(string caminho, DateTime primeiroDiaMes, DateTime ultimoDiaMes)
  {
    Dictionary<string, int> indicesCsv = new();
    List<FuncionarioVO> registroFuncionarios = new();
    int numeroLinha = 1;

    using (StreamReader sr = new(caminho))
    {
      while (!sr.EndOfStream)
      {
        string linha = await sr.ReadLineAsync() ?? string.Empty;
        try
        {
          string[] campos = linha?.Split(Separator) ?? Array.Empty<string>();
          if (numeroLinha == 1)
          {
            for (int i = 0; i < campos.Length; i++)
            {
              indicesCsv.Add(campos[i], i);
            }
          }
          else
          {
            FuncionarioVO funcionario = new(campos, indicesCsv);

            if (registroFuncionarios.Any(r => r.Data == funcionario.Data && r.Codigo == funcionario.Codigo && r.Nome == funcionario.Nome))
              throw new Exception($"Funcionário código {funcionario.Codigo} e nome {funcionario.Nome}, com mais de 1 registro para o dia {funcionario.Data}!");
            else if (funcionario.Data < primeiroDiaMes || funcionario.Data > ultimoDiaMes)
            {
              throw new Exception($"Data {funcionario.Data} inválida, esse registro não pertence à o mês/ano informado!");
            }
            else
              registroFuncionarios.Add(funcionario);
          }
        }
        catch (Exception ex)
        {
          Console.WriteLine($"Erro no arquivo {caminho} na linha {numeroLinha}: {ex.Message}");
        }
        finally
        {
          numeroLinha++;
        }
      }
    }

    return registroFuncionarios;
  }
}