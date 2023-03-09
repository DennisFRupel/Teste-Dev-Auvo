namespace TesteDevAuvo.ValueObjects;
public class FuncionarioVO
{
  public FuncionarioVO()
  {
    Codigo = string.Empty;
    Nome = string.Empty;
  }

  public FuncionarioVO(string[] campos, Dictionary<string, int> indicesCsv)
  {
    Codigo = campos[indicesCsv["Código"]];
    Nome = campos[indicesCsv["Nome"]];
    ValorHora = ParseValorHora(campos[indicesCsv["Valor hora"]]);
    Data = DateTime.Parse(campos[indicesCsv["Data"]]);
    Entrada = TimeSpan.Parse(campos[indicesCsv["Entrada"]]);
    Saida = TimeSpan.Parse(campos[indicesCsv["Saída"]]);
    Almoco = ParseHoraAlmoco(campos[indicesCsv["Almoço"]]);
  }

  static TimeSpan ParseHoraAlmoco(string rangeHora)
  {
    var valor = rangeHora.Split("-").Select(r => TimeSpan.Parse(r)).ToArray();
    return valor[1] - valor[0];
  }

  static double ParseValorHora(string valorHora)
  {
    string valorLimpo = valorHora.Replace("R$", "").Replace(" ", "");
    _ = double.TryParse(valorLimpo, out double valor);
    return valor;
  }

  public string Codigo { get; set; }
  public string Nome { get; set; }
  public double ValorHora { get; set; }
  public DateTime Data { get; set; }
  public TimeSpan Entrada { get; set; }
  public TimeSpan Saida { get; set; }
  public TimeSpan Almoco { get; set; }
}