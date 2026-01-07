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

namespace Specifications;

class MockScope1(
  IReadOnlyList<MockScope1> Included,
  IReadOnlyList<(MockScope1 Other, MockScope1 Result)> Ored,
  IReadOnlyList<(MockScope1 Other, MockScope1 Result)> Anded) : MatchScope<MockScope1>
{
  readonly IReadOnlyList<(MockScope1 Other, MockScope1 Result)> Anded = Anded;
  readonly IReadOnlyList<(MockScope1 Other, MockScope1 Result)> Ored = Ored;
  public static MockScope1 Any { get; } = new([], [], []);

  public static MockScope1 Unspecified { get; } = new([], [], []);

  public bool Includes(MockScope1 Other)
  {
    return Included.Contains(Other);
  }

  public static MockScope1 operator |(MockScope1 L, MockScope1 R)
  {
    return L.Ored.Single(O => O.Other == R).Result;
  }

  public static MockScope1 operator &(MockScope1 L, MockScope1 R)
  {
    return L.Anded.Single(O => O.Other == R).Result;
  }
}

class MockScope2(
  IReadOnlyList<MockScope2> Included,
  IReadOnlyList<(MockScope2 Other, MockScope2 Result)> Ored,
  IReadOnlyList<(MockScope2 Other, MockScope2 Result)> Anded) : MatchScope<MockScope2>
{
  readonly IReadOnlyList<(MockScope2 Other, MockScope2 Result)> Anded = Anded;
  readonly IReadOnlyList<(MockScope2 Other, MockScope2 Result)> Ored = Ored;
  public static MockScope2 Any => new([], [], []);

  public static MockScope2 Unspecified => new([], [], []);

  public bool Includes(MockScope2 Other)
  {
    return Included.Contains(Other);
  }

  public static MockScope2 operator |(MockScope2 L, MockScope2 R)
  {
    return L.Ored.Single(O => O.Other == R).Result;
  }

  public static MockScope2 operator &(MockScope2 L, MockScope2 R)
  {
    return L.Anded.Single(O => O.Other == R).Result;
  }
}