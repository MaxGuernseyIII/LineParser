namespace LineParser;

sealed class RecursiveExpression<Scope, Meaning>(Scope Demand) : Expression<Scope, Meaning>
  where Scope : MatchScope<Scope>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<Scope, Meaning> Reentry, MatchExecutionContext Context)
  {
    return Context.DoRecursive(ToMatch, this, () => Reentry.Match(ToMatch, Context, Demand).Select(M => M.Match));
  }
}