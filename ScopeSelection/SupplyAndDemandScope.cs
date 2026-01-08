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

namespace ScopeSelection;

/// <summary>
/// A kind of scope based on supply and demand of tokens.
/// </summary>
/// <typeparam name="T">The type of token.</typeparam>
public sealed record SupplyAndDemandScope<T> : Scope<SupplyAndDemandScope<T>>
{
  internal SupplyAndDemandScope(Predicate<Predicate<T>> CheckForSupport, Predicate<T> SupportsToken)
  {
    this.CheckForSupport = CheckForSupport;
    this.SupportsToken = SupportsToken;
  }

  readonly Predicate<Predicate<T>> CheckForSupport;
  readonly Predicate<T> SupportsToken;

  public bool IsSatisfiedBy(SupplyAndDemandScope<T> Other)
  {
    return CheckForSupport(Other.SupportsToken);
  }

  static bool Always(T _)
  {
    return true;
  }

  static bool Never(T _)
  {
    return false;
  }

  static bool Always(Predicate<T> _)
  {
    return true;
  }

  static bool Never(Predicate<T> _)
  {
    return false;
  }

  static Predicate<T> SupplyToken(T Required)
  {
    return Checked => Equals(Required, Checked);
  }

  static Predicate<Predicate<T>> DemandToken(T Required)
  {
    return Supplied => Supplied(Required);
  }

  public sealed class Space : ScopeSpace<SupplyAndDemandScope<T>>
  {
    public SupplyAndDemandScope<T> Any { get; } = new(Always, Always);

    public SupplyAndDemandScope<T> Unspecified { get; } = new(Always, Never);


    public SupplyAndDemandScope<T> Union(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
    {
      var LSupportsToken = L.SupportsToken;
      var RSupportsToken = R.SupportsToken;
      var LCheckForSupport = L.CheckForSupport;
      var RCheckForSupport = R.CheckForSupport;
      return new(Support => LCheckForSupport(Support) || RCheckForSupport(Support), Token => LSupportsToken(Token) || RSupportsToken(Token));
    }

    public SupplyAndDemandScope<T> Intersection(SupplyAndDemandScope<T> L, SupplyAndDemandScope<T> R)
    {
      var LSupportsToken = L.SupportsToken;
      var RSupportsToken = R.SupportsToken;
      var LCheckForSupport = L.CheckForSupport;
      var RCheckForSupport = R.CheckForSupport;
      return new(Support => LCheckForSupport(Support) && RCheckForSupport(Support), Token => LSupportsToken(Token) && RSupportsToken(Token));
    }


    public SupplyAndDemandScope<T> For(T Token)
    {
      return new(DemandToken(Token), SupplyToken(Token));
    }

    public SupplyAndDemandScope<T> Demand(T Token)
    {
      return new(DemandToken(Token), Never);
    }

    public SupplyAndDemandScope<T> Demand(IEnumerable<T> Tokens)
    {
      return new(DemandTokens(Tokens), Never);
    }

    Predicate<Predicate<T>> DemandTokens(IEnumerable<T> Tokens)
    {
      return Demanded => Tokens.All(Token => Demanded(Token));
    }

    public SupplyAndDemandScope<T> Supply(T Token)
    {
      return new(Never, SupplyToken(Token));
    }

    public SupplyAndDemandScope<T> Supply(IEnumerable<T> Tokens)
    {
      return new(Never, SupplyTokens(Tokens));
    }

    Predicate<T> SupplyTokens(IEnumerable<T> Tokens)
    {
      return Tokens.Contains;
    }
  }
}