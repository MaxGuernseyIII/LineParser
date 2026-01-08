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

sealed class RegexPattern<ScopeImplementation>(Regex Pattern) : Pattern<ScopeImplementation> where ScopeImplementation : Scope<ScopeImplementation>
{
  readonly Regex Pattern = new("^" + Pattern.ToString().TrimStart('^').TrimEnd('$') + "$", Pattern.Options);
  readonly AnythingPattern<ScopeImplementation> Core = new();

  public IEnumerable<Match> GetMatchesAtBeginningOf(string ToMatch, SubpatternMatcher<ScopeImplementation> Reentry,
    MatchExecutionContext Context)
  {
    return
      Core.GetMatchesAtBeginningOf(ToMatch, Reentry, Context)
        .Select(Match => (Match, RegexMatch: Pattern.Match(Match.Matched)))
        .Where(Pair => Pair.RegexMatch.Success)
        .Select(Pair => new Match()
        {
          Matched = Pair.RegexMatch.Value,
          Remainder = ToMatch.Substring(Pair.RegexMatch.Value.Length),
          Captured =
          [
            ..Pair.RegexMatch.Groups.Cast<Group>().Skip(1).Select(C => new Match.Capture
            {
              At = C.Index,
              Value = C.Value
            })
          ]
        });
  }

  public TResult Query<TResult>(GraphQuery<ScopeImplementation, TResult> Query)
  {
    return Query.QueryRegex(Pattern);
  }
}