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

namespace LineParser;

/// <summary>
///   A <see cref="ScopeSelection.MatchScope{Scope}" /> that combines two other
///   <see cref="ScopeSelection.MatchScope{Scope}" />s into two isolated dimensions.
///   Inclusion in the resulting combined scope requires the independent inclusion on the left and right dimensions.
/// </summary>
/// <example>
/// </example>
/// <param name="Left"></param>
/// <param name="Right"></param>
/// <typeparam name="TLeft"></typeparam>
/// <typeparam name="TRight"></typeparam>
public sealed class CompositeScope<TLeft, TRight>(TLeft Left, TRight Right) : MatchScope<CompositeScope<TLeft, TRight>>
  where TLeft : MatchScope<TLeft>
  where TRight : MatchScope<TRight>
{
  public TLeft Left { get; } = Left;
  public TRight Right { get; } = Right;

  public bool IsSatisfiedBy(CompositeScope<TLeft, TRight> Other)
  {
    return Left.IsSatisfiedBy(Other.Left) && Right.IsSatisfiedBy(Other.Right);
  }

  public sealed class Space(MatchScopeSpace<TLeft> Left, MatchScopeSpace<TRight> Right)
    : MatchScopeSpace<CompositeScope<TLeft, TRight>>
  {
    public CompositeScope<TLeft, TRight> Any { get; } = new(Left.Any, Right.Any);

    public CompositeScope<TLeft, TRight> Unspecified { get; } = new(Left.Unspecified, Right.Unspecified);

    public CompositeScope<TLeft, TRight> Or(
      CompositeScope<TLeft, TRight> L,
      CompositeScope<TLeft, TRight> R)
    {
      return new(Left.Or(L.Left, R.Left), Right.Or(L.Right, R.Right));
    }

    public CompositeScope<TLeft, TRight> And(
      CompositeScope<TLeft, TRight> L,
      CompositeScope<TLeft, TRight> R)
    {
      return new(Left.And(L.Left, R.Left), Right.And(L.Right, R.Right));
    }

    public CompositeScope<TLeft, TRight> Combine(TLeft Left, TRight Right)
    {
      return new(Left, Right);
    }
  }
}