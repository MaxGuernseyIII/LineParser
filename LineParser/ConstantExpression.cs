namespace LineParser;

public class ConstantExpression(string Value)
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch)
  {
    yield return new()
    {
      Matched = Value,
      Remainder = ToMatch[Value.Length..]
    };
  }
}