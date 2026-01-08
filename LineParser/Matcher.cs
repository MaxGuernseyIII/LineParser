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
/// Scans a string for all possible matches using a set of <see cref="Pattern{ScopeImplementation}"/>s.
/// </summary>
/// <typeparam name="ScopeImplementation">The type of <see cref="Scope{Implementation}"/> used to select the <see cref="Pattern{ScopeImplementation}"/>s used in a match operation.</typeparam>
/// <typeparam name="Meaning">The type of object used to define the meaning of a match.</typeparam>
public interface Matcher<
  ScopeImplementation,
  Meaning>
  where ScopeImplementation : Scope<ScopeImplementation>
{
  /// <summary>
  /// The <see cref="ScopeSpace{ScopeImplementation}"/> for this <see cref="Matcher{ScopeImplementation,Meaning}"/>.
  /// </summary>
  ScopeSpace<ScopeImplementation> ScopeSpace { get; }

  /// <summary>
  /// The patterns used in this <see cref="Matcher{ScopeImplementation,Meaning}"/>.
  /// </summary>
  IEnumerable<Pattern<ScopeImplementation>> Patterns { get; }

  /// <summary>
  /// Finds all the possible matches for the input string. You should not call this directly and instead should use an extension method from <see cref="MatcherExtensions"/>.
  /// </summary>
  /// <param name="ToParse">The string to match.</param>
  /// <param name="Context">A mutable state object that use used during parsing.</param>
  /// <param name="Scope">The definition of the set of <see cref="Pattern{ScopeImplementation}"/>s to use.</param>
  /// <returns>All the possible <see cref="LineParser.Match"/>es with their associated meanings.</returns>
  IEnumerable<AnnotatedMatch<ScopeImplementation, Meaning>> Match(string ToParse, MatchExecutionContext Context, ScopeImplementation Scope);
}