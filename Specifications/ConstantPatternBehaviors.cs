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

[TestClass]
public sealed class ConstantPatternBehaviors
{
  Factory<NullScope> Factory = null!;

  [TestInitialize]
  public void SetUp()
  {
    Factory = ScopeSpaces.Null.GetFactory();
  }

  [TestMethod]
  public void ExactMatchRequiresWholeString()
  {
    var ToMatch = Any.String();

    var Pattern = Factory.Constant(ToMatch);
    var Matcher = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Pattern]);

    var Matches = Matcher.ExactMatch(ToMatch).Select(M => M.Match);
    Matches.ShouldBe([
      new()
      {
        Matched = ToMatch,
        Remainder = "",
        Captured = []
      }
    ]);
  }

  [TestMethod]
  public void ExactMatchFailsWithAnyDifferences()
  {
    var ToMatch = Any.String();

    var Pattern = Factory.Constant(ToMatch);
    var Matcher = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Pattern]);

    var Matches = Matcher.ExactMatch(ToMatch + Any.String());
    Matches.ShouldBe([]);
  }

  [TestMethod]
  public void MatchesBeginningOfStringWithRemainder()
  {
    var ToMatch = Any.String();
    var Remainder = Any.String();

    var Pattern = Factory.Constant(ToMatch);
    var Matcher = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Pattern]);

    var Matches = Matcher.Match(ToMatch + Remainder).Select(M => M.Match);
    Matches.ShouldBe([
      new()
      {
        Matched = ToMatch,
        Remainder = Remainder,
        Captured = []
      }
    ]);
  }

  [TestMethod]
  public void DoesNotMatchIfFragmentNotPresent()
  {
    var ToMatch = Any.String();
    var Remainder = Any.String();

    var Pattern = Factory.Constant(ToMatch);
    var Matcher = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Pattern]);

    var Matches = Matcher.Match(Any.String() + Remainder);
    Matches.ShouldBe([]);
  }

  [TestMethod]
  public void DoesNotMatchFragmentsAfterBeginning()
  {
    var ToMatch = Any.String();
    var Remainder = Any.String();

    var Pattern = Factory.Constant(ToMatch);
    var Matcher = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Pattern]);

    var Matches = Matcher.Match(Any.String() + ToMatch + Remainder);
    Matches.ShouldBe([]);
  }
}