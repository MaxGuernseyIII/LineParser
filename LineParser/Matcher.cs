using System.Collections.Immutable;

namespace LineParser;

public class Matcher(ImmutableArray<ConstantExpression> Expressions)
{
  public IEnumerable<Match> Match(string ToParse)
  {
    yield return new()
    {
      Matched = ToParse,
      Remainder = ""
    };
  }
}

public readonly record struct Match
{
  public Match() {}

  public required string Matched { get; init; }
  public required string Remainder { get; init; }
  public ImmutableArray<string> Captured { get; init; } = ImmutableArray<string>.Empty;
}