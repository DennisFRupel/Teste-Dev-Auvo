using System.Collections.Concurrent;

namespace TesteDevAuvo.Extensions;
public static class DateTimeExtensions
{
  private static ConcurrentDictionary<int, List<DateTime>> DictionayFeriados { get; set; } = new ConcurrentDictionary<int, List<DateTime>>();

  public static int GetDiasUteis(this DateTime date)
  {
    int ano = date.Year;
    int mes = date.Month;
    DateTime data = new(ano, mes, 1);
    int diasUteis = 0;
    while (data.Month == mes)
    {
      if (IsDiaUtil(data))
      {
        diasUteis++;
      }
      data = data.AddDays(1);
    }
    return diasUteis;
  }

  public static bool IsDiaUtil(this DateTime data)
  {
    List<DateTime> feriados = ObterFeriadosNacionais(data.Year);

    return data.DayOfWeek != DayOfWeek.Saturday
      && data.DayOfWeek != DayOfWeek.Sunday
      && !feriados.Contains(data);
  }

  private static List<DateTime> ObterFeriadosNacionais(int ano)
  {
    if (!DictionayFeriados.ContainsKey(ano))
    {
      DateTime pascoa = CalcularPascoa(ano);

      List<DateTime> feriados = new()
      {
        // Feriados Fixos
        new DateTime(ano, 1, 1), // Ano novo
        new DateTime(ano, 4, 21), // Tiradentes
        new DateTime(ano, 5, 1), // Dia do Trabalho
        new DateTime(ano, 9, 7), // Independência do Brasil
        new DateTime(ano, 10, 12), // Nossa Senhora Aparecida
        new DateTime(ano, 11, 2), // Finados
        new DateTime(ano, 11, 15), // Proclamação da República
        new DateTime(ano, 12, 25), // Natal
      
        // Feriados Móveis
        pascoa.AddDays(-2), // Sexta-Feira da Paixão
        pascoa,
        pascoa.AddDays(60) // Corpus Christi
      };

      DictionayFeriados.TryAdd(ano, feriados);
    }

    return DictionayFeriados[ano];
  }

  private static DateTime CalcularPascoa(int ano)
  {
    int a = ano % 19;
    int b = ano / 100;
    int c = ano % 100;
    int d = b / 4;
    int e = b % 4;
    int f = (b + 8) / 25;
    int g = (b - f + 1) / 3;
    int h = ((19 * a) + b - d - g + 15) % 30;
    int i = c / 4;
    int k = c % 4;
    int l = (32 + (2 * e) + (2 * i) - h - k) % 7;
    int m = (a + (11 * h) + (22 * l)) / 451;
    int mes = (h + l - (7 * m) + 114) / 31;
    int dia = ((h + l - (7 * m) + 114) % 31) + 1;
    return new DateTime(ano, mes, dia);
  }
}