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

using StringScope = SupplyAndDemandScope<string>;

[TestClass]
public class GraphQueryBehavior
{
  Factory<StringScope> Factory = null!;
  StringScope.Space ScopeSpace = null!;

  [TestInitialize]
  public void Setup()
  {
    ScopeSpace = ScopeSpaces.SupplyAndDemand<string>();
    Factory = ScopeSpace.GetFactory();
  }

  [TestMethod]
  public void MatcherTreatsPatternsAsAlternatives()
  {
    var Matcher = Factory.Matcher([
      new MockPattern<StringScope>(), new MockPattern<StringScope>()
    ]);

    var Actual = Matcher.Query(new TestGraphQuery<StringScope, IEnumerable<Pattern<StringScope>>>()
    {
      OnQueryPatternAlternatives = P => P
    });

    Actual.ShouldBe(Matcher.Patterns);
  }

  [TestMethod]
  public void ParallelTreatsPatternsAsAlternatives()
  {
    IEnumerable<Pattern<StringScope>> Patterns = [
      new MockPattern<StringScope>(), new MockPattern<StringScope>()
    ];
    var Node = Factory.Parallel(Patterns);

    var Actual = Node.Query(new TestGraphQuery<StringScope, IEnumerable<Pattern<StringScope>>>()
    {
      OnQueryPatternAlternatives = P => P
    });

    Actual.ShouldBe(Patterns);
  }

  [TestMethod]
  public void ConstantQuery()
  {
    var Content = Any.String();
    var Node = Factory.Constant(Content);

    var Actual = Node.Query(new TestGraphQuery<StringScope, string>()
    {
      OnQueryConstant = C => C
    });

    Actual.ShouldBe(Content);
  }

  [TestMethod]
  public void Capture()
  {
    var Inner = new MockPattern<StringScope>();
    var Node = Factory.Capturing(Inner);

    var Actual = Node.Query(new TestGraphQuery<StringScope, Pattern<StringScope>>()
    {
      OnQueryCapturing = I => I
    });

    Actual.ShouldBe(Inner);
  }

  [TestMethod]
  public void Anything()
  {
    var Node = Factory.Anything();

    var Actual = Node.Query(new TestGraphQuery<StringScope, int>()
    {
      OnQueryAnything = () => 1
    });

    Actual.ShouldBe(1);
  }

  [TestMethod]
  public void Sequence()
  {
    IEnumerable<Pattern<StringScope>> Patterns =
    [
      new MockPattern<StringScope>(), new MockPattern<StringScope>()
    ];

    var Node = Factory.Sequence(Patterns);

    var Actual = Node.Query(new TestGraphQuery<StringScope, IEnumerable<Pattern<StringScope>>>()
    {
      OnQuerySequence = P => P
    });

    Actual.ShouldBe(Patterns);
  }

  [TestMethod]
  public void Subpattern()
  {
    var Scope = ScopeSpace.Demand(Any.String());
    var Node = Factory.Subpattern(Scope);

    var Actual = Node.Query(new TestGraphQuery<StringScope, StringScope>()
    {
      OnQuerySubpattern = S => S
    });

    Actual.ShouldBe(Scope);
  }

  [TestMethod]
  public void Regex()
  {
    var Regex = new Regex("^" + Any.String() + "$");
    var Node = Factory.Regex(Regex);

    var Actual = Node.Query(new TestGraphQuery<StringScope, Regex>()
    {
      OnQueryRegex = R => R
    });

    Actual.ToString().ShouldBe(Regex.ToString());
    Actual.Options.ShouldBe(Regex.Options);
  }

  [TestMethod]
  public void ToSimplifiedRegex()
  {
    var Pattern = Factory.Sequence([
      Factory.Constant("a"),
      Factory.Anything(),
      Factory.Capturing(Factory.Constant("b")),
      Factory.Parallel([
        Factory.Constant("c"),
        Factory.Constant("d"),
      ]),
      Factory.Regex(new("^e|f$")),
      Factory.Subpattern(ScopeSpace.For(Any.String()))
    ]);

    var Regex = Pattern.ToSimplifiedRegexString();

    Regex.ShouldBe("a.*(b)(?:c|d)(?:e|f).*");
  }
}