namespace LineParser;

public class CapturingExpression(Expression ToCaptureExpression) : Expression
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher Reentry, MatchExecutionContext Context)
  {
    return ToCaptureExpression.GetMatchesAtBeginningOf(ToMatch, Reentry, Context)
      .Select(M => M with {Captured = [M.Matched]});
  }
}