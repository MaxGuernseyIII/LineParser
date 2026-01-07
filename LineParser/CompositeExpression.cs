namespace LineParser;

public class CompositeExpression(IEnumerable<Expression> Expressions) : Expression
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher Reentry, MatchExecutionContext Context)
  {
    IEnumerable<Match> Result= [
      new()
      {
        Matched = "",
        Remainder = ToMatch,
        Captured = []
      }
    ];

    foreach (var Expression in Expressions)
      Result = Result.SelectMany(Left =>
        Expression.GetMatchesAtBeginningOf(Left.Remainder, Reentry, Context)
          .Select(Right => Left + Right));

    return Result;
  }
}