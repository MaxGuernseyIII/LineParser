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
using LineParser;

namespace Specifications;

class TestGraphQuery<TScope, T> : GraphQuery<TScope, T>
  where TScope : Scope<TScope>
{
  static T Fail<U>(U _)
  {
    Assert.Fail();
    return default!;
  }
  static T Fail()
  {
    Assert.Fail();
    return default!;
  }

  public Func<IEnumerable<Pattern<TScope>>, T> OnQueryPatternAlternatives { get; set; } = Fail;
  public Func<string, T> OnQueryConstant { get; set; } = Fail;
  public Func<Pattern<TScope>, T> OnQueryCapturing { get; set; } = Fail;
  public Func<T> OnQueryAnything { get; set; } = Fail;
  public Func<IEnumerable<Pattern<TScope>>, T> OnQuerySequence { get; set; } = Fail;
  public Func<TScope, T> OnQuerySubpattern { get; set; } = Fail;
  public Func<Regex, T> OnQueryRegex { get; set; } = Fail;

  public T QueryAlternativePatterns(IEnumerable<Pattern<TScope>> Alternatives)
  {
    return OnQueryPatternAlternatives(Alternatives);
  }

  public T QueryConstant(string Content)
  {
    return OnQueryConstant(Content);
  }

  public T QueryCapturing(Pattern<TScope> Inner)
  {
    return OnQueryCapturing(Inner);
  }

  public T QueryAnything()
  {
    return OnQueryAnything();
  }

  public T QuerySequence(IEnumerable<Pattern<TScope>> Steps)
  {
    return OnQuerySequence(Steps);
  }

  public T QuerySubpattern(TScope Demanded)
  {
    return OnQuerySubpattern(Demanded);
  }

  public T QueryRegex(Regex Regex)
  {
    return OnQueryRegex(Regex);
  }
}