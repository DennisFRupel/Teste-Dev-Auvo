namespace TesteDevAuvo.Extensions;
public static class TimeSpanExtensions
{
  public static TimeSpan Sum(this IEnumerable<TimeSpan> times)
  {
    TimeSpan timeSpan = TimeSpan.Zero;
    foreach (TimeSpan time in times)
    {
      timeSpan += time;
    }
    return timeSpan;
  }
}