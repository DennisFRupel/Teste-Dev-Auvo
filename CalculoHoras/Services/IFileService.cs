using TesteDevAuvo.Dominio;
using TesteDevAuvo.ValueObjects;

namespace TesteDevAuvo.Services;
public interface IFileService
{
  Task<List<RegistroPagamento>> ReadFilesAsync(string caminhoPasta);
  Task<RegistroPagamento> ReadFileAsync(string caminho);
  Task<List<FuncionarioVO>> ConverteArquivo(string caminho, DateTime primeiroDiaMes, DateTime ultimoDiaMes);
}