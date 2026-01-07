namespace LineParser;

public class CapturingExpression<T>(Expression<T> ToCaptureExpression) : Expression<T> where T : MatchScope<T>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<T> Reentry, MatchExecutionContext Context)
  {
    return ToCaptureExpression.GetMatchesAtBeginningOf(ToMatch, Reentry, Context)
      .Select(M => M with {Captured = [M.Matched]});
  }
}