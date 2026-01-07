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

public sealed record SupplyAndDemandScope<T> : MatchScope<SupplyAndDemandScope<T>>
{
  public required Predicate<Predicate<T>> Demand { get; init; }
  public required Predicate<T> Supply { get; init; }

  public static SupplyAndDemandScope<T> Any { get; } = new()
  {
    Demand = _ => true,
    Supply = _ => true
  };

  public static SupplyAndDemandScope<T> Unspecified { get; } = new()
  {
    Demand = _ => true,
    Supply = _ => false
  };

  public bool Includes(SupplyAndDemandScope<T> Other)
  {
    return Demand(Other.Supply);
  }

  public static SupplyAndDemandScope<T> operator |(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
  {
    throw new NotImplementedException();
  }

  public static SupplyAndDemandScope<T> operator &(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
  {
    throw new NotImplementedException();
  }

  public static SupplyAndDemandScope<T> For(T Token)
  {
    return new()
    {
      Demand = Other => Other(Token),
      Supply = OtherToken => Equals(Token, OtherToken)
    };
  }
}