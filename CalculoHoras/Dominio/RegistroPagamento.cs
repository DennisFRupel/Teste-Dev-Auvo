namespace TesteDevAuvo.Dominio;
public class RegistroPagamento
{
  public RegistroPagamento()
  {
    Departamento = string.Empty;
    MesVigencia = string.Empty;
    Funcionarios = new List<Funcionario>();
  }

  public string Departamento { get; set; }
  public string MesVigencia { get; set; }
  public int AnoVigencia { get; set; }
  public double TotalPagar { get; set; }
  public double TotalDescontos { get; set; }
  public double TotalExtras { get; set; }
  public List<Funcionario> Funcionarios { get; set; }
}