using TesteDevAuvo.Dominio;
using TesteDevAuvo.Extensions;
using TesteDevAuvo.ValueObjects;

namespace TesteDevAuvo.Services.Impl;
public class RegistroPagamentoService : IRegistroPagamentoService
{
  public RegistroPagamentoService() { }

  public RegistroPagamento ProcessaDadosEntrada(RegistroPagamento departamento, List<FuncionarioVO> funcionarios, int quantidadeDiasUteis)
  {
    TimeSpan quantidadeNormalHoras = new TimeSpan(8, 0, 0);

    var dadosProcessados = funcionarios.GroupBy(r => new { r.Codigo, r.Nome, r.ValorHora }, (key, g) => new
    {
      key.Codigo,
      key.Nome,
      key.ValorHora,
      DiasTrabalhados = g.Count(),
      DiasExtras = g.Count(r => !r.Data.IsDiaUtil()),
      DiasFalta = g.Count(r => r.Data.IsDiaUtil()) - quantidadeDiasUteis,
      HorasDebito = (g.Where(r => r.Data.IsDiaUtil() &&
                                       (r.Saida - r.Entrada - r.Almoco) < quantidadeNormalHoras)
                                                                     .Select(r => quantidadeNormalHoras - (r.Saida - r.Entrada - r.Almoco)).Sum()
                                                      + ((g.Count(r => r.Data.IsDiaUtil()) - quantidadeDiasUteis) * quantidadeNormalHoras)).TotalHours,
      HorasExtras = (g.Where(r => r.Data.IsDiaUtil() &&
                                       (r.Saida - r.Entrada - r.Almoco) > quantidadeNormalHoras)
                                                                     .Select(r => r.Saida - r.Entrada - r.Almoco - quantidadeNormalHoras).Sum()
                                                      + g.Where(r => !r.Data.IsDiaUtil())
                                                                     .Select(r => r.Saida - r.Entrada - r.Almoco).Sum()).TotalHours,
      HorasTotal = g.Select(r => r.Saida - r.Entrada - r.Almoco).Sum().TotalHours
    }).ToList();

    departamento.Funcionarios = dadosProcessados.Select(r => new Funcionario
    {
      Codigo = r.Codigo,
      Nome = r.Nome,
      DiasTrabalhados = r.DiasTrabalhados,
      DiasExtras = r.DiasExtras,
      DiasFalta = r.DiasFalta,
      HorasDebito = r.HorasDebito,
      HorasExtras = r.HorasExtras,
      TotalReceber = r.HorasTotal * r.ValorHora
    }).ToList();

    departamento.TotalPagar = dadosProcessados.Sum(r => r.HorasTotal * r.ValorHora);
    departamento.TotalDescontos = dadosProcessados.Sum(r => r.HorasDebito * r.ValorHora);
    departamento.TotalExtras = dadosProcessados.Sum(r => r.HorasExtras * r.ValorHora);
    return departamento;
  }
}