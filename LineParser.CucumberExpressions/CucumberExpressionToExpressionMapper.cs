using CucumberExpressions.Ast;
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

    //return ExpressionFactory.CreateConstant(Expression);
    return ConvertNodesToExpression(Tree.Nodes, GetScopeForString);
  }

  Expression<Scope, Meaning> ConvertNodesToExpression(IEnumerable<Node> Nodes, Func<string, Scope> ScopeForString)
  {
    var Parts = new List<Expression<Scope, Meaning>>();

    foreach (var Node in Nodes)
    {
      Parts.Add(Node.Type switch
      {
        NodeType.TEXT_NODE => ExpressionFactory.CreateConstant(Node.Text),
        NodeType.OPTIONAL_NODE => ExpressionFactory.CreateAlternatives(
        [
          ConvertNodesToExpression(Node.Nodes, ScopeForString),
          ExpressionFactory.CreateConstant("")
        ]),

        NodeType.ALTERNATION_NODE => ExpressionFactory.CreateAlternatives(
          Node.Nodes.Select(N => ConvertNodesToExpression(N.Nodes, ScopeForString))
          ),
        NodeType.ALTERNATIVE_NODE => ConvertNodesToExpression(Node.Nodes, ScopeForString),
        NodeType.PARAMETER_NODE => ExpressionFactory.CreateCapturing(ExpressionFactory.CreateRecursive(ScopeForString(Node.Text))),
        NodeType.EXPRESSION_NODE => ConvertNodesToExpression(Node.Nodes, ScopeForString),
        _ => throw new ArgumentOutOfRangeException($"Unknown node type: {Node.Type}")
      });
    }

    return ExpressionFactory.CreateComposite(Parts);
  }
}