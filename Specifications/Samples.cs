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
public class Samples
{
  [TestMethod]
  public void FirstStep()
  {
    var Factory = MatchScopeSpaces.Null.GetFactory();
    var MeatPattern = Factory.Constant("meat");
    var Matcher = Factory.Matcher([MeatPattern]);

    Matcher.ExactMatch("meat").Count().ShouldBe(1);
    Matcher.ExactMatch("cheese").Count().ShouldBe(0);
  }

  [TestMethod]
  public void AddingMeaning()
  {
    var Factory = MatchScopeSpaces.Null.GetFactory();
    var MeatPattern = Factory.Constant("meat");
    var TheMeaningOfMeat = BinderFactory.ForMeat();
    var Matcher = Factory.Matcher([(MeatPattern, TheMeaningOfMeat)]);

    Matcher.ExactMatch("meat").Single().Meaning.ShouldBeSameAs(TheMeaningOfMeat);
  }

  [TestMethod]
  public void AddingScope()
  {
    var ScopeSpace = MatchScopeSpaces.SupplyAndDemand<string>();
    var Factory = ScopeSpace.GetFactory();
    var MeatPattern = Factory.Constant("meat");
    var TheMeaningOfMeat = BinderFactory.ForMeat();
    var Matcher = Factory.Matcher([(ScopeSpace.Supply("food"), MeatPattern, TheMeaningOfMeat)]);

    Matcher.ExactMatch("meat", ScopeSpace.Demand("food")).Count().ShouldBe(1);
    Matcher.ExactMatch("meat", ScopeSpace.Demand("airplanes")).Count().ShouldBe(0);
  }

  [TestMethod]
  public void MatchWithConditionals()
  {
    var ExpressionFactory = MatchScopeSpaces.Null.GetFactory();
    var Matcher = ExpressionFactory.Matcher(
    [
      (ExpressionFactory.Composite([
          ExpressionFactory.Constant("user \""),
          ExpressionFactory.Capturing(
            ExpressionFactory.Regex(new("(?:[^\"]|\"\")*"))),
          ExpressionFactory.Constant("\"")
        ]),
        "username")
    ]);

    var Matches = Matcher.ExactMatch("user \"jumper9\"");

    Matches.ShouldBe([
      new()
      {
        Match = new()
        {
          Matched = "user \"jumper9\"",
          Remainder = "",
          Captured =
          [
            new()
            {
              At = 6,
              Value = "jumper9"
            }
          ]
        },
        Meaning = "username"
      }
    ]);
  }

  class Binder
  {
  }

  static class BinderFactory
  {
    public static Binder ForMeat()
    {
      return new();
    }
  }
}