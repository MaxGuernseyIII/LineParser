namespace LineParser;

sealed class RecursiveExpression<Scope, Meaning>(Scope Demand) : Expression<Scope, Meaning>
  where Scope : MatchScope<Scope>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<Scope, Meaning> Reentry, MatchExecutionContext Context)
  {
    yield break;
  }
}