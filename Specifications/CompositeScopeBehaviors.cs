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
using Microsoft.Testing.Platform.Extensions;
using Shouldly;

namespace Specifications;

[TestClass]
public class CompositeScopeBehaviors
{
  [TestMethod]
  public void AnyIsComposed()
  {
    CompositeScope<MockScope1, MockScope2>.Any.ShouldBeEquivalentTo(
      new CompositeScope<MockScope1, MockScope2>(MockScope1.Any, MockScope2.Any));
  }

  [TestMethod]
  public void UnspecifiedIsComposed()
  {
    CompositeScope<MockScope1, MockScope2>.Unspecified.ShouldBeEquivalentTo(
      new CompositeScope<MockScope1, MockScope2>(MockScope1.Unspecified, MockScope2.Unspecified));
  }

  [TestMethod]
  public void IncludesWhenBothLeftAndRightInclude()
  {
    var Left = new MockScope1([], [], []);
    var Right = new MockScope2([], [], []);
    var Composed = new CompositeScope<MockScope1, MockScope2>(
      new([Left], [], []),
      new([Right], [], [])
    );

    Composed.Includes(new(Left, Right)).ShouldBeTrue();
  }

  [TestMethod]
  public void IncludesWhenLeftAIncludes()
  {
    var Left = new MockScope1([], [], []);
    var Right = new MockScope2([], [], []);
    var Composed = new CompositeScope<MockScope1, MockScope2>(
      new([Left], [], []),
      new([], [], [])
    );

    Composed.Includes(new(Left, Right)).ShouldBeFalse();
  }

  [TestMethod]
  public void IncludesWhenRightIncludes()
  {
    var Left = new MockScope1([], [], []);
    var Right = new MockScope2([], [], []);
    var Composed = new CompositeScope<MockScope1, MockScope2>(
      new([], [], []),
      new([Right], [], [])
    );

    Composed.Includes(new(Left, Right)).ShouldBeFalse();
  }

  [TestMethod]
  public void IncludesWhenNeitherIncludes()
  {
    var Left = new MockScope1([], [], []);
    var Right = new MockScope2([], [], []);
    var Composed = new CompositeScope<MockScope1, MockScope2>(
      new([], [], []),
      new([], [], [])
    );

    Composed.Includes(new(Left, Right)).ShouldBeFalse();
  }

  [TestMethod]
  public void Or()
  {
    var NewLeft = new MockScope1([], [], []);
    var NewRight = new MockScope2([], [], []);
    var RightLeft = new MockScope1([], [], []);
    var RightRight = new MockScope2([], [], []);
    var LeftLeft = new MockScope1([], [(RightLeft, NewLeft)], []);
    var LeftRight = new MockScope2([], [(RightRight, NewRight)], []);

    var Actual = new CompositeScope<MockScope1, MockScope2>(LeftLeft, LeftRight) |
                 new CompositeScope<MockScope1, MockScope2>(RightLeft, RightRight);

    Actual.ShouldBeEquivalentTo(new CompositeScope<MockScope1, MockScope2>(NewLeft, NewRight));
  }
}