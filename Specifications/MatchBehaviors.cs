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
public class MatchBehaviors
{
  [TestMethod]
  public void AppendMatches()
  {
    var Match1 = Any.Match();
    var Match2 = Any.Match();

    var Actual = Match1 + Match2;

    Actual.Matched.ShouldBe(Match1.Matched + Match2.Matched);
    Actual.Remainder.ShouldBe(Match2.Remainder);
    Actual.Captured.ShouldBe([.. Match1.Captured, .. Match2.Captured]);
  }

  [TestMethod]
  public void MatchEquivalence()
  {
    var Match = Any.Match();

    var OtherMatch = new Match
    {
      Matched = Match.Matched,
      Remainder = Match.Remainder,
      Captured = [..Match.Captured]
    };

    OtherMatch.ShouldBe(Match);
  }
}