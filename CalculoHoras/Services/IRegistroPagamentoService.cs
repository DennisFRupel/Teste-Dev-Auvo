using TesteDevAuvo.Dominio;
using TesteDevAuvo.ValueObjects;

namespace TesteDevAuvo.Services;
public interface IRegistroPagamentoService
{
  RegistroPagamento ProcessaDadosEntrada(RegistroPagamento departamento, List<FuncionarioVO> funcionarios, int quantidadeDiasUteis);
}