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

using System.Text.RegularExpressions;
using LineParser;
using Shouldly;

namespace Specifications;

[TestClass]
public class RegexAdapterBehaviors
{
  [TestMethod]
  public void UsesRegexToParse()
  {
    var Remainder = Any.String();
    var ToMatch = "there is some cheese in the house ";
    var ToParse = ToMatch + Remainder;
    var Pattern = new Regex("there is (some|no) cheese in the (house|refrigerator) ", RegexOptions.Compiled);
    var Expression = new PatternFactory<NullScope, object>().Regex(Pattern);

    var Matches = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Expression]).Match(ToParse)
      .Select(M => M.Match);

    Matches.ShouldBe([
      new()
      {
        Matched = ToMatch,
        Remainder = Remainder,
        Captured =
        [
          new()
          {
            At = 9,
            Value = "some"
          },
          new()
          {
            At = 28,
            Value = "house"
          }
        ]
      }
    ]);
  }

  [TestMethod]
  public void OnlyMatchesAtCurrentLocation()
  {
    var ToMatch = "there is some cheese in the house ";
    var ToParse = Any.String() + ToMatch + Any.String();
    var Pattern = new Regex("there is (some|no) cheese in the (house|refrigerator) ", RegexOptions.Compiled);
    var Expression = new PatternFactory<NullScope, object>().Regex(Pattern);

    var Matches = TestMatcherFactory.CreateFromExpressionsWithoutMeaning([Expression]).Match(ToParse);

    Matches.ShouldBe([]);
  }
}