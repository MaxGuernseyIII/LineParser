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
  static bool Always(T _) => true;
  static bool Never(T _) => false;
  static bool Always(Predicate<T> _) => true;
  static bool Never(Predicate<T> _) => false;
  static Predicate<T> SupplyToken(T Required) => Checked => Equals(Required, Checked);
  static Predicate<Predicate<T>> DemandToken(T Required) => Supplied => Supplied(Required);

  public required Predicate<Predicate<T>> CheckForSupport { get; init; }
  public required Predicate<T> SupportsToken { get; init; }

  public static SupplyAndDemandScope<T> Any { get; } = new()
  {
    CheckForSupport = Always,
    SupportsToken = Always
  };

  public static SupplyAndDemandScope<T> Unspecified { get; } = new()
  {
    CheckForSupport = Always,
    SupportsToken = Never
  };

  public bool Includes(SupplyAndDemandScope<T> Other)
  {
    return CheckForSupport(Other.SupportsToken);
  }

  public static SupplyAndDemandScope<T> operator |(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
  {
    var LSupportsToken = L.SupportsToken;
    var RSupportsToken = R.SupportsToken;
    var LCheckForSupport = L.CheckForSupport;
    var RCheckForSupport = R.CheckForSupport;
    return new()
    {
      CheckForSupport = Support => LCheckForSupport(Support) || RCheckForSupport(Support),
      SupportsToken = Token => LSupportsToken(Token) || RSupportsToken(Token)
    };
  }

  public static SupplyAndDemandScope<T> operator &(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
  {
    var LSupportsToken = L.SupportsToken;
    var RSupportsToken = R.SupportsToken;
    var LCheckForSupport = L.CheckForSupport;
    var RCheckForSupport = R.CheckForSupport;
    return new()
    {
      CheckForSupport = Support => LCheckForSupport(Support) && RCheckForSupport(Support),
      SupportsToken = Token => LSupportsToken(Token) && RSupportsToken(Token)
    };
  }

  public static SupplyAndDemandScope<T> For(T Token)
  {
    return new()
    {
      CheckForSupport = DemandToken(Token),
      SupportsToken = SupplyToken(Token)
    };
  }

  public static SupplyAndDemandScope<T> Demand(T Token)
  {
    return new()
    {
      SupportsToken = Never,
      CheckForSupport = DemandToken(Token)
    };
  }

  public static SupplyAndDemandScope<T> Supply(T Token)
  {
    return new()
    {
      SupportsToken = SupplyToken(Token),
      CheckForSupport = Never
    };
  }
}