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

using System.Collections.Immutable;
using System.Text;

namespace LineParser;

/// <summary>
/// A single candidate for matching a string. Note: It is only a complete match if it has an empty <see cref="Remainder"/>.
/// </summary>
public readonly record struct Match
{
  /// <summary>
  /// Initialize the object.
  /// </summary>
  public Match()
  {
  }

  /// <summary>
  /// The complete string that was matched from its beginning up to the point where matching either completed or failed.
  /// </summary>
  public required string Matched { get; init; }

  /// <summary>
  /// The portion after <see cref="Matched"/> that was not matched by the <see cref="Pattern{ScopeImplementation}"/> or <see cref="Matcher{ScopeImplementation,Meaning}"/>.
  /// </summary>
  public required string Remainder { get; init; }

  /// <summary>
  /// The list of captured strings (in order).
  /// </summary>
  public ImmutableArray<Capture> Captured { get; init; } = ImmutableArray<Capture>.Empty;

  /// <inheritdoc />
  public bool Equals(Match Other)
  {
    return Matched == Other.Matched && Remainder == Other.Remainder && Captured.SequenceEqual(Other.Captured);
  }

  /// <summary>
  /// Concatenates <see cref="Left"/> and <see cref="Right"/>.
  /// </summary>
  /// <param name="Left">The first match in the concatenation.</param>
  /// <param name="Right">The match to be appended.</param>
  /// <returns>The resulting <see cref="Match"/></returns>
  public static Match operator +(Match Left, Match Right)
  {
    return new()
    {
      Matched = Left.Matched + Right.Matched,
      Remainder = Right.Remainder,
      Captured =
      [
        ..Left.Captured,
        ..Right.Captured.Select(C => C with {At = Left.Matched.Length + C.At})
      ]
    };
  }

  /// <inheritdoc />
  public override int GetHashCode()
  {
    unchecked
    {
      var Hash = 17;
      Hash = Hash * 23 + (Matched?.GetHashCode() ?? 0);
      Hash = Hash * 23 + Remainder.GetHashCode();
      return Hash;
    }
  }

  bool PrintMembers(StringBuilder Builder)
  {
    Builder.Append(
      $"Matched = \"{Matched}\", Remainder = \"{Remainder}\", Captured = [{string.Join(", ", Captured)}]");

    return true;
  }

  public readonly record struct Capture
  {
    public required int At { get; init; }
    public required string Value { get; init; }
  }
}