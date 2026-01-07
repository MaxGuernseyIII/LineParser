namespace LineParser;

sealed class CapturingExpression<Scope, Meaning>(Expression<Scope, Meaning> ToCaptureExpression) : Expression<Scope, Meaning> where Scope : MatchScope<Scope>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<Scope, Meaning> Reentry, MatchExecutionContext Context)
  {
    return ToCaptureExpression.GetMatchesAtBeginningOf(ToMatch, Reentry, Context)
      .Select(M => M with {Captured = [new()
      {
        At = 0,
        Value = M.Matched
      }]});
  }
}