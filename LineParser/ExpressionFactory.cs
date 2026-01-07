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

using System.Runtime.Serialization;
using System.Text.RegularExpressions;

namespace LineParser;

public class ExpressionFactory<T>
  where T : MatchScope<T>
{
  public Expression<T> CreateRecursive(T Demand)
  {
    return new RecursiveExpression<T>(Demand);
  }

  public Expression<T> CreateCapturing(Expression<T> ToCaptureExpression)
  {
    return new CapturingExpression<T>(ToCaptureExpression);
  }

  public Expression<T> CreateComposite(IEnumerable<Expression<T>> Expressions)
  {
    return new CompositeExpression<T>(Expressions);
  }

  public Expression<T> CreateConstant(string Value)
  {
    return new ConstantExpression<T>(Value);
  }

  public Expression<T> CreateForRegex(Regex Pattern)
  {
    return new RegexExpression<T>(Pattern);
  }
}