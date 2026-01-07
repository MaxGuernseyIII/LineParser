using System.Collections.Immutable;

namespace LineParser;

public class Matcher(ImmutableArray<ConstantExpression> Expressions)
{
  public IEnumerable<Match> Match(IParsable<string> ToParse)
  {
    yield return new() {Remainder = ""};
  }
}

public readonly record struct Match
{
  public required string Remainder { get; init; }
}