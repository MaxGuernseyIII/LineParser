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
/// Extensions for <see cref="Pattern{ScopeImplementation}"/>
/// </summary>
public static class PatternExtensions
{
  class SimplifiedRegexQuery<TScope> : GraphQuery<TScope, string> where TScope 
    : Scope<TScope>
  {
    public string QueryAlternativePatterns(IEnumerable<Pattern<TScope>> Alternatives)
    {
      return $"(?:{string.Join("|", Alternatives.Select(P => P.Query(this)))})";
    }

    public string QueryConstant(string Content)
    {
      return Regex.Escape(Content);
    }

    public string QueryCapturing(Pattern<TScope> Inner)
    {
      return "(" + Inner.Query(this) + ")";
    }

    public string QueryAnything()
    {
      return ".*";
    }

    public string QuerySequence(IEnumerable<Pattern<TScope>> Steps)
    {
      return string.Join("", Steps.Select(S => S.Query(this)));
    }

    public string QuerySubpattern(TScope Demanded)
    {
      return ".*";
    }

    public string QueryRegex(Regex Regex)
    {
      return $"(?:{Regex.ToString().TrimEnd('$').TrimStart('^')})";
    }

    public string QueryOther(Pattern<TScope> Other)
    {
      return "";
    }
  }

  extension<ScopeImplementation>(Pattern<ScopeImplementation> This) where ScopeImplementation : Scope<ScopeImplementation>
  {
    /// <summary>
    /// Convert to a simplified regular expression.
    ///
    /// Specifically, complex structures are replaced with <c>"(.*)"</c>. This guarantees that the simplified regular expression will always match anything matched by this object, but it may also match some other strings as well.
    /// </summary>
    /// <returns>The expression.</returns>
    public string ToSimplifiedRegexString()
    {
      return This.Query(new SimplifiedRegexQuery<ScopeImplementation>());
    }
  }
}