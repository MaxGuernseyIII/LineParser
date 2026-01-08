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
/// The definition of a pattern, which is something that can match part of a string.
/// </summary>
/// <typeparam name="ScopeImplementation">The kind of scope used to select <see cref="Pattern{ScopeImplementation}"/>s for a match operation.</typeparam>
public interface Pattern<out ScopeImplementation>
  where ScopeImplementation : Scope<ScopeImplementation>
{
  /// <summary>
  /// Finds all <see cref="Match"/>es that start at the beginning of <paramref name="ToMatch"/>.
  /// </summary>
  /// <param name="ToMatch">The string to match.</param>
  /// <param name="FindSubpattern">The operation that can be used to match a subpattern.</param>
  /// <param name="Context">The state of the parse operation.</param>
  /// <returns>All valid <see cref="Match"/>es.</returns>
  IEnumerable<Match> GetMatchesAtBeginningOf(
    string ToMatch, SubpatternMatcher<ScopeImplementation> FindSubpattern, MatchExecutionContext Context);
}