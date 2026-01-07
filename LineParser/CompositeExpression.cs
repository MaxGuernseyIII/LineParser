namespace LineParser;

public class CompositeExpression(IEnumerable<Expression> Expressions) : Expression
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher Reentry, MatchExecutionContext Context)
  {
    yield break;
  }
}