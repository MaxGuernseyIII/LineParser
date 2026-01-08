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
/// Methods that simplify use of <see cref="Matcher{ScopeImplementation,Meaning}"/>.
/// </summary>
public static class MatcherExtensions
{
  extension<Scope, Meaning>(Matcher<Scope, Meaning> This) where Scope : Scope<Scope>
  {
    /// <summary>
    /// Find all matches that start at the beginning of <paramref name="ToParse"/>.
    /// </summary>
    /// <param name="ToParse">The string to match.</param>
    /// <returns>All partial and complete matches that start at index <c>0</c> of <paramref name="ToParse"/>.</returns>
    public IEnumerable<AnnotatedMatch<Meaning>> Match(string ToParse)
    {
      return This.Match(ToParse, This.ScopeSpace.Any);
    }

    /// <summary>
    /// Find all matches that start at the beginning of <paramref name="ToParse"/>, but only for patterns that are in <paramref name="MatchScope"/>.
    /// </summary>
    /// <param name="ToParse">The string to match.</param>
    /// <param name="MatchScope">The <see cref="Scope{Implementation}"/> that selects the <see cref="Pattern{ScopeImplementation}"/>s which will be used to perform the match operation.</param>
    /// <returns>All partial and complete matches that start at index <c>0</c> of <paramref name="ToParse"/>.</returns>
    public IEnumerable<AnnotatedMatch<Meaning>> Match(string ToParse, Scope MatchScope)
    {
      return This.Match(ToParse, new(), MatchScope);
    }

    /// <summary>
    /// Find all matches that start at the beginning of <paramref name="ToParse"/> and match the entire string.
    /// </summary>
    /// <param name="ToParse">The string to match.</param>
    /// <returns>All partial and complete matches that start at index <c>0</c> of <paramref name="ToParse"/>.</returns>
    public IEnumerable<AnnotatedMatch<Meaning>> ExactMatch(string ToParse)
    {
      return This.ExactMatch(ToParse, This.ScopeSpace.Any);
    }

    /// <summary>
    /// Find all matches that start at the beginning of <paramref name="ToParse"/> and match the entire string, but only for patterns that are in <paramref name="MatchScope"/>.
    /// </summary>
    /// <param name="ToParse">The string to match.</param>
    /// <param name="MatchScope">The <see cref="Scope{Implementation}"/> that selects the <see cref="Pattern{ScopeImplementation}"/>s which will be used to perform the match operation.</param>
    /// <returns>All partial and complete matches that start at index <c>0</c> of <paramref name="ToParse"/>.</returns>
    public IEnumerable<AnnotatedMatch<Meaning>> ExactMatch(string ToParse, Scope MatchScope)
    {
      return This.Match(ToParse, MatchScope).Where(M => M.Match.Remainder == "");
    }
  }
}