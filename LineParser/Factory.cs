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

public class Factory<Scope>(ScopeSpace<Scope> ScopeSpace) where Scope : Scope<Scope>
{
  public ScopeSpace<Scope> ScopeSpace { get; } = ScopeSpace;

  public Matcher<Scope, object> Matcher(
    IEnumerable<Pattern<Scope>> Patterns)
  {
    return Matcher(Patterns.Select(P => (P, new object())));
  }

  public Matcher<Scope, Meaning> Matcher<Meaning>(
    IEnumerable<(Pattern<Scope> Pattern, Meaning Meaning)> PatternsWithMeanings)
  {
    return Matcher(PatternsWithMeanings.Select(P => (ScopeSpace.Any, P.Pattern, P.Meaning)));
  }

  public Matcher<Scope, Meaning> Matcher<Meaning>(
    IEnumerable<(Scope Scope, Pattern<Scope> Pattern, Meaning Meaning)> PatternsWithMeanings)
  {
    return new MatcherImplementation<Scope, Meaning>(
      ScopeSpace,
      [
        ..PatternsWithMeanings
      ]);
  }

  public Pattern<Scope> Anything()
  {
    return new AnythingPattern<Scope>();
  }

  public Pattern<Scope> Constant(string Value)
  {
    return new ConstantPattern<Scope>(Value);
  }

  public Pattern<Scope> Composite(IEnumerable<Pattern<Scope>> Patterns)
  {
    return new CompositePattern<Scope>(Patterns);
  }

  public Pattern<Scope> Alternatives(IEnumerable<Pattern<Scope>> Patterns)
  {
    return new Alternatives<Scope>(Patterns);
  }

  public Pattern<Scope> Capturing(Pattern<Scope> ToCapturePattern)
  {
    return new CapturingPattern<Scope>(ToCapturePattern);
  }

  public Pattern<Scope> SubPattern(Scope Demand)
  {
    return new RecursivePattern<Scope>(Demand);
  }

  public Pattern<Scope> Regex(Regex Pattern)
  {
    return new RegexPattern<Scope>(Pattern);
  }
}