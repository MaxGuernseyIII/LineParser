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

using LineParser;
using Shouldly;

namespace Specifications;

using StringScope = SupplyAndDemandScope<string>;

[TestClass]
public class RecursiveExpressionBehaviors
{
  ExpressionFactory<StringScope, object> ExpressionFactory = null!;

  [TestInitialize]
  public void Setup()
  {
    ExpressionFactory = new();
  }

  [TestMethod]
  public void InvokesInScopeExpressions()
  {
    var InnerScope = Any.String();
    var OuterScope = Any.String();
    var MatchedStub = Any.String();
    var ToParse = MatchedStub + Any.String();
    var Recursive = ExpressionFactory.CreateRecursive(StringScope.Demand(InnerScope));
    var Constant = ExpressionFactory.CreateConstant(MatchedStub);
    var Matcher = TestMatcherFactory.CreateFromRegistryWithoutMeaning([
      (StringScope.Supply(InnerScope), Constant),
      (StringScope.Supply(OuterScope),  Recursive)
    ]);

    var Actual = Matcher.Match(ToParse, new(), StringScope.Demand(OuterScope)).Select(M => M.Match);

    Actual.ShouldBe(Constant.GetMatchesAtBeginningOf(ToParse, Matcher, new()));
  }
}