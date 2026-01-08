// MIT License
// 
// Copyright (c) 2026-2026 Producore LLC
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using CucumberExpressions.Ast;
using CucumberExpressions.Parsing;

namespace LineParser.CucumberExpressions;

public class CucumberExpressionToExpressionMapper<Scope, Meaning>(MatchScopeSpace<Scope> ScopeSpace)
  where Scope : MatchScope<Scope>
{
  readonly Factory<Scope> Factory = ScopeSpace.Get().PatternFactory();
  readonly CucumberExpressionParser Parser = new();

  public Pattern<Scope> Map(
    string Expression,
    Func<string, Scope> GetScopeForString)
  {
    var Tree = Parser.Parse(Expression);

    return ConvertNodesToExpression(Tree.Nodes, GetScopeForString);
  }

  Pattern<Scope> ConvertNodesToExpression(IEnumerable<Node> Nodes, Func<string, Scope> ScopeForString)
  {
    var Parts = new List<Pattern<Scope>>();

    foreach (var Node in Nodes)
      Parts.Add(Node.Type switch
      {
        NodeType.TEXT_NODE => Factory.Constant(Node.Text),
        NodeType.OPTIONAL_NODE => Factory.Alternatives(
        [
          ConvertNodesToExpression(Node.Nodes, ScopeForString),
          Factory.Constant("")
        ]),

        NodeType.ALTERNATION_NODE => Factory.Alternatives(
          Node.Nodes.Select(N => ConvertNodesToExpression(N.Nodes, ScopeForString))
        ),
        NodeType.ALTERNATIVE_NODE => ConvertNodesToExpression(Node.Nodes, ScopeForString),
        NodeType.PARAMETER_NODE => Factory.Capturing(
          Factory.Recursive(ScopeForString(Node.Text))),
        NodeType.EXPRESSION_NODE => ConvertNodesToExpression(Node.Nodes, ScopeForString),
        _ => throw new ArgumentOutOfRangeException($"Unknown node type: {Node.Type}")
      });

    return Factory.Composite(Parts);
  }
}