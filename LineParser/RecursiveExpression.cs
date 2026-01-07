namespace LineParser;

class RecursiveExpression<T>(T Demand) : Expression<T>
  where T : MatchScope<T>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<T> Reentry, MatchExecutionContext Context)
  {
    yield break;
  }
}