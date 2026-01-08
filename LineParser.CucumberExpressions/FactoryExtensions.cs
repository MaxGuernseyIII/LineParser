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

using ScopeSelection;

namespace LineParser.CucumberExpressions;

/// <summary>
/// Extensions for <see cref="Factory{ScopeImplementation}"/>.
/// </summary>
public static class FactoryExtensions
{
  extension<Scope>(Factory<Scope> This) where Scope : Scope<Scope>
  {
    /// <summary>
    /// Parse a Cucumber expression and convert it into a <see cref="Pattern{ScopeImplementation}"/>.
    /// </summary>
    /// <param name="Template">The expression template.</param>
    /// <param name="GetScopeForParameterName">A way to find scopes for Cucumber expression parameter names.</param>
    /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
    public Pattern<Scope> CucumberExpression(string Template, Func<string, Scope> GetScopeForParameterName)
    {
      return new CucumberExpressionToExpressionMapper<Scope>(This).Map(Template, GetScopeForParameterName);
    }
  }
}