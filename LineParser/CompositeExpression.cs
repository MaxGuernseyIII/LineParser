namespace LineParser;

public class CompositeExpression<T>(IEnumerable<Expression<T>> Expressions) : Expression<T> where T : MatchScope<T>
{
  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, Matcher<T> Reentry, MatchExecutionContext Context)
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