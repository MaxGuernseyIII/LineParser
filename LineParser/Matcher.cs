using System.Collections.Immutable;

namespace LineParser;

public class Matcher(ImmutableArray<ConstantExpression> Expressions)
{
  public IEnumerable<Match> Match(string ToParse)
  {
    foreach (var Expression in Expressions)
    foreach (var Match in Expression.GetMatchesAtBeginningOf(ToParse))
    {
      yield return Match;
    }
  }
}