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
public sealed class CompositeExpressionBehaviors
{
  [TestMethod]
  public void ChainsExpressions()
  {
    var OverallString = Any.String();

    Match[] FirstMatches =
    [
      Any.Match(),
      Any.Match()
    ];
    var MatchesForRemainder0 = Any.ArrayOf(Any.Match);
    var MatchesForRemainder1 = Any.ArrayOf(Any.Match);
    var FinalMatches = Any.ArrayOf(Any.Match);

    var FirstMatch0 = FirstMatches[0];
    var FirstMatch1 = FirstMatches[1];
    var Expression = new CompositeExpression([
      new MockExpression
      {
        Results =
        {
          {OverallString, FirstMatches}
        }
      },
      new MockExpression
      {
        Results =
        {
          {FirstMatch0.Remainder, MatchesForRemainder0},
          {FirstMatch1.Remainder, MatchesForRemainder1}
        }
      },
      new MockExpression
      {
        Results = MatchesForRemainder0.Concat(MatchesForRemainder1).Select(R => R.Remainder).Distinct()
          .ToDictionary(Key => Key, IEnumerable<Match> (_) => FinalMatches)
      }
    ]);
    var Matcher = new Matcher([Expression]);

    var Actual = Matcher.Match(OverallString);

    var Expected = MatchesForRemainder0.SelectMany(Level2 => FinalMatches.Select(Level3 => FirstMatch0 + Level2 + Level3))
      .Concat(MatchesForRemainder1.SelectMany(Level2 => FinalMatches.Select(Level3 => FirstMatch1 + Level2 + Level3)));

    Actual.ShouldBe(Expected, true);
  }
}