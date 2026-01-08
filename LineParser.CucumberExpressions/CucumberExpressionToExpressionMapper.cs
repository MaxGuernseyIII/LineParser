using CucumberExpressions.Parsing;

namespace LineParser.CucumberExpressions;

public class CucumberExpressionToExpressionMapper<Scope, Meaning> where Scope : MatchScope<Scope>
{
  readonly ExpressionFactory<Scope, Meaning> ExpressionFactory = new();
  readonly CucumberExpressionParser Parser = new();

  public Expression<Scope, Meaning> Map(
    string Expression, 
    Func<string, Scope> GetScopeForString)
  {
    var Tree = Parser.Parse(Expression);

    return ExpressionFactory.CreateConstant(Expression);
  }
}