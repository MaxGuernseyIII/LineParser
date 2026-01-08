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
using ScopeSelection;

namespace LineParser.CucumberExpressions;

class CucumberExpressionToExpressionMapper<Scope>(Factory<Scope> Factory)
  where Scope : MatchScope<Scope>
{
  readonly CucumberExpressionParser Parser = new();

  public Pattern<Scope> Map(
    string Template,
    Func<string, Scope> GetScopeForString)
  {
    var Tree = Parser.Parse(Template);

    return ConvertNodesToPattern(Tree.Nodes, GetScopeForString);
  }

  Pattern<Scope> ConvertNodesToPattern(IEnumerable<Node> Nodes, Func<string, Scope> ScopeForString)
  {
    var Parts = new List<Pattern<Scope>>();

    foreach (var Node in Nodes)
      Parts.Add(Node.Type switch
      {
        NodeType.TEXT_NODE => Factory.Constant(Node.Text),
        NodeType.OPTIONAL_NODE => Factory.Alternatives(
        [
          ConvertNodesToPattern(Node.Nodes, ScopeForString),
          Factory.Constant("")
        ]),

        NodeType.ALTERNATION_NODE => Factory.Alternatives(
          Node.Nodes.Select(N => ConvertNodesToPattern(N.Nodes, ScopeForString))
        ),
        NodeType.ALTERNATIVE_NODE => ConvertNodesToPattern(Node.Nodes, ScopeForString),
        NodeType.PARAMETER_NODE => Factory.Capturing(
          Factory.SubPattern(ScopeForString(Node.Text))),
        NodeType.EXPRESSION_NODE => ConvertNodesToPattern(Node.Nodes, ScopeForString),
        _ => throw new ArgumentOutOfRangeException($"Unknown node type: {Node.Type}")
      });

    return Factory.Composite(Parts);
  }
}