using ScopeSelection;

namespace LineParser.CucumberExpressions;

public static class FactoryExtensions
{
  extension<Scope>(Factory<Scope> This) where Scope : MatchScope<Scope>
  {
    public Pattern<Scope> CucumberExpression(string Template, Func<string, Scope> GetScopeForParameterName)
    {
      var Mapper = new CucumberExpressionToExpressionMapper<Scope>(This);
      return Mapper.Map(Template, GetScopeForParameterName);
    }
  }
}