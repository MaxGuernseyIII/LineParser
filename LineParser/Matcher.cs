using System.Collections.Immutable;

namespace LineParser;

public class Matcher(ImmutableArray<ConstantExpression> Expressions)
{
  public IEnumerable<Match> Match(string ToParse)
  {
    yield return new()
    {
      Matched = ToParse,
      Remainder = ""
    };
  }
}