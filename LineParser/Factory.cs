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

/// <summary>
/// A factory that simplifies construction for when using LineParser.
/// </summary>
/// <param name="ScopeSpace">The scope space that will be used by all objects the factory creates.</param>
/// <typeparam name="ScopeImplementation">The type of scope to use.</typeparam>
public class Factory<ScopeImplementation>(ScopeSpace<ScopeImplementation> ScopeSpace) where ScopeImplementation : Scope<ScopeImplementation>
{
  /// <summary>
  /// The scope space for this factory.
  /// </summary>
  public ScopeSpace<ScopeImplementation> ScopeSpace { get; } = ScopeSpace;

  /// <summary>
  /// Creates a <see cref="Matcher{ScopeImplementation, Meaning}"/> with no scopes or meanings assigned to any pattern.
  /// </summary>
  /// <param name="Patterns">The set of <see cref="Pattern{ScopeImplementation}"/>s to match.</param>
  /// <returns>The requested <see cref="Matcher{ScopeImplementation,Meaning}"/>.</returns>
  public Matcher<ScopeImplementation, object> Matcher(
    IEnumerable<Pattern<ScopeImplementation>> Patterns)
  {
    return Matcher(Patterns.Select(P => (P, new object())));
  }

  /// <summary>
  /// Creates a <see cref="Matcher{ScopeImplementation, Meaning}"/> with meanings assigned to each pattern.
  /// </summary>
  /// <param name="PatternsWithMeanings">The set of <see cref="Pattern{ScopeImplementation}"/>s to match with associated meanings.</param>
  /// <typeparam name="Meaning">The type of object that represents the meaning of a <see cref="Pattern{ScopeImplementation}"/>.</typeparam>
  /// <returns>The requested <see cref="Matcher{ScopeImplementation,Meaning}"/>.</returns>
  public Matcher<ScopeImplementation, Meaning> Matcher<Meaning>(
    IEnumerable<(Pattern<ScopeImplementation> Pattern, Meaning Meaning)> PatternsWithMeanings)
  {
    return Matcher(PatternsWithMeanings.Select(P => (ScopeSpace.Any, P.Pattern, P.Meaning)));
  }

  /// <summary>
  /// Creates a <see cref="Matcher{ScopeImplementation, Meaning}"/> with meanings and constraining <see cref="Scope{Implementation}"/>s assigned to each pattern.
  /// </summary>
  /// <param name="PatternsWithMeanings">The set of <see cref="Pattern{ScopeImplementation}"/>s to match with associated meanings and constraining scopes.</param>
  /// <typeparam name="Meaning">The type of object that represents the meaning of a <see cref="Pattern{ScopeImplementation}"/>.</typeparam>
  /// <returns>The requested <see cref="Matcher{ScopeImplementation,Meaning}"/>.</returns>
  public Matcher<ScopeImplementation, Meaning> Matcher<Meaning>(
    IEnumerable<(ScopeImplementation Scope, Pattern<ScopeImplementation> Pattern, Meaning Meaning)> PatternsWithMeanings)
  {
    return new MatcherImplementation<ScopeImplementation, Meaning>(
      ScopeSpace,
      [
        ..PatternsWithMeanings
      ]);
  }

  /// <summary>
  /// Matches literally anything. This will always produce a set of options that represents all possible substrings that start at index 0.
  ///
  /// For instance, assuming you use it to parse <c>"abc"</c>, you will get the following in your matches:
  ///
  /// <code>
  /// [
  ///   "abc",
  ///   "ab",
  ///   "a"
  ///   ""
  /// ]
  ///
  /// This is the stand-in for <see cref="System.Text.RegularExpressions.Regex"/>'s interpretation of <c>"(.*)"</c>.
  /// </code>
  /// </summary>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Anything()
  {
    return new AnythingPattern<ScopeImplementation>();
  }

  /// <summary>
  /// Matches an exact string. If the current location starts with the specified string, it will match. It not, this object produces no matches.
  /// </summary>
  /// <param name="Value"></param>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Constant(string Value)
  {
    return new ConstantPattern<ScopeImplementation>(Value);
  }

  /// <summary>
  /// Concatenates matches. Runs the first pattern normally and each successive pattern is applied to the resulting output.
  ///
  /// Therefore, to produce matches, all patterns must match a string in sequence.
  ///
  /// The patterns <b>cannot</b> all match at the same point in the string. They must match in sequence.
  ///
  /// This is a peer to <see cref="System.Text.RegularExpressions.Regex"/>'s ordered layout of expressions. <em>e.g.</em>, <c>"(?:pattern1)(?:pattern2)"</c>.
  /// </summary>
  /// <seealso cref="Parallel"/>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Sequence(IEnumerable<Pattern<ScopeImplementation>> Patterns)
  {
    return new CompositePattern<ScopeImplementation>(Patterns);
  }

  /// <summary>
  /// Provides alternate patterns. All patterns are applied to the current parsing location and their results are accumulated into a single result set.
  ///
  /// The patterns <b>cannot</b> match in sequence. They all are applied to the same starting point.
  ///
  /// This is a peer to <see cref="System.Text.RegularExpressions.Regex"/>'s alternative expressions. <em>e.g.</em>, <c>"(?:pattern1|pattern2)"</c>.
  ///
  /// It is also similar to Cucumber expressions alternatives (<c>"this/that"</c>).
  /// </summary>
  /// <param name="Patterns"></param>
  /// <seealso cref="Sequence"/>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Parallel(IEnumerable<Pattern<ScopeImplementation>> Patterns)
  {
    return new Alternatives<ScopeImplementation>(Patterns);
  }

  /// <summary>
  /// Obliterates any captures from the underlying <see cref="Pattern{ScopeImplementation}"/> and captures its resulting matched strings instead.
  ///
  /// This is a peer to <see cref="System.Text.RegularExpressions.Regex"/>'s capturing group. <em>e.g.</em>, <c>"(pattern1)"</c>.
  ///
  /// It also parallels Cucumber expressions' parameters (<c>"{string}"</c>).
  /// </summary>
  /// <param name="ToCapturePattern">The <see cref="Pattern{ScopeImplementation}"/> for which output is captured.</param>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Capturing(Pattern<ScopeImplementation> ToCapturePattern)
  {
    return new CapturingPattern<ScopeImplementation>(ToCapturePattern);
  }

  /// <summary>
  /// Reapply the containing <see cref="Matcher"/> with a given scope to find a subpattern.
  ///
  /// There is no peer to this concept in <see cref="System.Text.RegularExpressions.Regex"/>. You can find something similar in PCRE-compatible regex libraries, though.
  /// </summary>
  /// <param name="Demand"></param>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Subpattern(ScopeImplementation Demand)
  {
    return new Subpattern<ScopeImplementation>(Demand);
  }

  /// <summary>
  /// Adapt a <see cref="System.Text.RegularExpressions.Regex"/> into the graph and expose its match and captures in the results.
  /// </summary>
  /// <param name="Pattern">The <see cref="System.Text.RegularExpressions.Regex"/> to adapt.</param>
  /// <returns>The requested <see cref="Pattern{ScopeImplementation}"/>.</returns>
  public Pattern<ScopeImplementation> Regex(Regex Pattern)
  {
    return new RegexPattern<ScopeImplementation>(Pattern);
  }
}