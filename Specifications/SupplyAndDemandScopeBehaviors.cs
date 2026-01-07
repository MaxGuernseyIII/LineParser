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

using TestedScope = SupplyAndDemandScope<MockToken>;

[TestClass]
public class SupplyAndDemandScopeBehaviors
{
  [TestMethod]
  public void AnyIncludesItself()
  {
    TestedScope.Any.Includes(TestedScope.Any).ShouldBeTrue();
  }

  [TestMethod]
  public void AnyIncludesToken()
  {
    TestedScope.Any.Includes(TestedScope.For(new())).ShouldBeTrue();
  }

  [TestMethod]
  public void AnyIncludesUnspecified()
  {
    TestedScope.Any.Includes(TestedScope.Unspecified).ShouldBeTrue();
  }

  [TestMethod]
  public void RequireIncludesAny()
  {
    TestedScope.Demand(new()).Includes(TestedScope.Any).ShouldBeTrue();
  }

  [TestMethod]
  public void RequireIncludesToken()
  {
    var Token = new MockToken();
    TestedScope.Demand(Token).Includes(TestedScope.For(Token)).ShouldBeTrue();
  }

  [TestMethod]
  public void RequireIncludesSameSupply()
  {
    var Token = new MockToken();
    TestedScope.Demand(Token).Includes(TestedScope.Supply(Token)).ShouldBeTrue();
  }

  [TestMethod]
  public void RequireDoesNotIncludeOtherToken()
  {
    TestedScope.Demand(new()).Includes(TestedScope.For(new())).ShouldBeFalse();
  }

  [TestMethod]
  public void RequireDoesNotIncludeOtherSupply()
  {
    TestedScope.Demand(new()).Includes(TestedScope.Supply(new())).ShouldBeFalse();
  }

  [TestMethod]
  public void RequireDoesNotIncludeUnspecified()
  {
    TestedScope.Demand(new()).Includes(TestedScope.Unspecified).ShouldBeFalse();
  }

  [TestMethod]
  public void UnspecifiedIncludesItself()
  {
    TestedScope.Unspecified.Includes(TestedScope.Unspecified).ShouldBeTrue();
  }

  [TestMethod]
  public void UnspecifiedIncludesToken()
  {
    TestedScope.Unspecified.Includes(TestedScope.For(new())).ShouldBeTrue();
  }

  [TestMethod]
  public void UnspecifiedIncludesAny()
  {
    TestedScope.Unspecified.Includes(TestedScope.Any).ShouldBeTrue();
  }

  [TestMethod]
  public void TokenIncludesAny()
  {
    TestedScope.For(new()).Includes(TestedScope.Any).ShouldBeTrue();
  }

  [TestMethod]
  public void TokenIncludesSameToken()
  {
    var Token = new MockToken();
    TestedScope.For(Token).Includes(TestedScope.For(Token)).ShouldBeTrue();
  }

  [TestMethod]
  public void TokenDoesNotIncludeOtherToken()
  {
    TestedScope.For(new()).Includes(TestedScope.For(new())).ShouldBeFalse();
  }

  [TestMethod]
  public void TokenDoesNotIncludeUnspecified()
  {
    TestedScope.For(new()).Includes(TestedScope.Unspecified).ShouldBeFalse();
  }

  [TestMethod]
  public void AndWhenBothDemandsAreSupplied()
  {
    MockToken Token1 = new();
    MockToken Token2 = new();

    (TestedScope.Demand(Token1) & TestedScope.Demand(Token2))
      .Includes(TestedScope.Supply(Token1) | TestedScope.Supply(Token2)).ShouldBeTrue();
  }

  [TestMethod]
  public void AndWhenLeftDemandsAreSupplied()
  {
    MockToken Token1 = new();
    MockToken Token2 = new();

    (TestedScope.Demand(Token1) & TestedScope.Demand(Token2))
      .Includes(TestedScope.Supply(Token1)).ShouldBeFalse();
  }

  [TestMethod]
  public void AndWhenRightDemandsAreSupplied()
  {
    MockToken Token1 = new();
    MockToken Token2 = new();

    (TestedScope.Demand(Token1) & TestedScope.Demand(Token2))
      .Includes(TestedScope.Supply(Token2)).ShouldBeFalse();
  }
}

class MockToken
{
}