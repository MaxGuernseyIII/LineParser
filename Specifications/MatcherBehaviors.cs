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
public class MatcherBehaviors
{
  Factory<StringScope> Factory = null!;
  StringScope.Space ScopeSpace = null!;

  [TestInitialize]
  public void Setup()
  {
    ScopeSpace = MatchScopeSpaces.SupplyAndDemand<string>();
    Factory = ScopeSpace.GetFactory();
  }

  [TestMethod]
  public void SuppliesAnnotatedMeaning()
  {
    var ToMatch = Any.String();
    var Meaning = new object();
    var Matcher = Factory.Matcher([
      (Factory.Constant(ToMatch), Meaning)
    ]);

    var Actual = Matcher.Match(ToMatch);

    Actual.Single().Meaning.ShouldBe(Meaning);
  }

  [TestMethod]
  public void ConstrainsPossibilitiesByScope()
  {
    var ToMatch = Any.String();
    var TargetScope = Any.String();
    var Meaning = new object();
    var Matcher = Factory.Matcher(
    [
      (ScopeSpace.Supply(Any.String()), Factory.Constant(ToMatch), new()),
      (ScopeSpace.Supply(TargetScope), Factory.Constant(ToMatch), Meaning),
      (ScopeSpace.Supply(Any.String()), Factory.Constant(ToMatch), new())
    ]);

    var Actual = Matcher.Match(ToMatch, new(), ScopeSpace.Demand(TargetScope)).Select(M => M.Meaning);

    Actual.ShouldBe([
      Meaning
    ]);
  }
}