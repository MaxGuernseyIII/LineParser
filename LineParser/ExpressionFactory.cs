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

using System.Text.RegularExpressions;

namespace LineParser;

public class ExpressionFactory<Scope, Meaning>
  where Scope : MatchScope<Scope>
{
  public Pattern<Scope, Meaning> CreateRecursive(Scope Demand)
  {
    return new RecursivePattern<Scope, Meaning>(Demand);
  }

  public Pattern<Scope, Meaning> CreateCapturing(Pattern<Scope, Meaning> ToCapturePattern)
  {
    return new CapturingPattern<Scope, Meaning>(ToCapturePattern);
  }

  public Pattern<Scope, Meaning> CreateComposite(IEnumerable<Pattern<Scope, Meaning>> Expressions)
  {
    return new CompositePattern<Scope, Meaning>(Expressions);
  }

  public Pattern<Scope, Meaning> CreateConstant(string Value)
  {
    return new ConstantPattern<Scope, Meaning>(Value);
  }

  public Pattern<Scope, Meaning> CreateForRegex(Regex Pattern)
  {
    return new RegexPattern<Scope, Meaning>(Pattern);
  }

  public Pattern<Scope, Meaning> CreateAlternatives(IEnumerable<Pattern<Scope, Meaning>> Expressions)
  {
    return new Alternatives<Scope, Meaning>(Expressions);
  }

  public Pattern<Scope, Meaning> CreateMatchAnything()
  {
    return new AnythingPattern<Scope, Meaning>();
  }
}