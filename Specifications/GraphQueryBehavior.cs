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
public class GraphQueryBehavior
{
  Factory<NullScope> Factory = null!;

  [TestInitialize]
  public void Setup()
  {
    Factory = ScopeSpaces.Null.GetFactory();
  }

  [TestMethod]
  public void MatcherTreatsPatternsAsAlternatives()
  {
    var Matcher = Factory.Matcher([
      new MockPattern<NullScope>(), new MockPattern<NullScope>()
    ]);

    var Actual = Matcher.Query(new TestGraphQuery<NullScope, IEnumerable<Pattern<NullScope>>>()
    {
      OnQueryPatternAlternatives = P => P
    });

    Actual.ShouldBe(Matcher.Patterns);
  }

  [TestMethod]
  public void ParallelTreatsPatternsAsAlternatives()
  {
    IEnumerable<Pattern<NullScope>> Patterns = [
      new MockPattern<NullScope>(), new MockPattern<NullScope>()
    ];
    var Node = Factory.Parallel(Patterns);

    var Actual = Node.Query(new TestGraphQuery<NullScope, IEnumerable<Pattern<NullScope>>>()
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

    var Actual = Node.Query(new TestGraphQuery<NullScope, string>()
    {
      OnQueryConstant = C => C
    });

    Actual.ShouldBe(Content);
  }

  [TestMethod]
  public void Capture()
  {
    var Inner = new MockPattern<NullScope>();
    var Node = Factory.Capturing(Inner);

    var Actual = Node.Query(new TestGraphQuery<NullScope, Pattern<NullScope>>()
    {
      OnQueryCapturing = I => I
    });

    Actual.ShouldBe(Inner);
  }

  [TestMethod]
  public void Anything()
  {
    var Node = Factory.Anything();

    var Actual = Node.Query(new TestGraphQuery<NullScope, int>()
    {
      OnQueryAnything = () => 1
    });

    Actual.ShouldBe(1);
  }

  [TestMethod]
  public void Sequence()
  {
    IEnumerable<Pattern<NullScope>> Patterns =
    [
      new MockPattern<NullScope>(), new MockPattern<NullScope>()
    ];

    var Node = Factory.Sequence(Patterns);

    var Actual = Node.Query(new TestGraphQuery<NullScope, IEnumerable<Pattern<NullScope>>>()
    {
      OnQuerySequence = P => P
    });

    Actual.ShouldBe(Patterns);
  }
}