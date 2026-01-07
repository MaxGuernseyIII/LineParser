namespace LineParser;

public record StringScope : MatchScope<StringScope>
{
  public required Predicate<Predicate<string>> Demanded { get; init; }
  public required Predicate<string> Supplied { get; init; } 

  public static StringScope Any { get; } = new()
  {
    Demanded = _ => true,
    Supplied = _ => true
  };

  public static StringScope Unspecified { get; } = new()
  {
    Demanded = _ => true,
    Supplied = _ => false
  };

  public bool Includes(StringScope Other)
  {
    return Demanded(Other.Supplied);
  }

  public static StringScope operator |(StringScope L, StringScope R)
  {
    return new()
    {
      Demanded = Other => L.Demanded(Other) || R.Demanded(Other),
      Supplied = Other => L.Supplied(Other) || R.Supplied(Other)
    };
  }

  public static StringScope operator &(StringScope L, StringScope R)
  {
    return new()
    {
      Demanded = Other => L.Demanded(Other) && R.Demanded(Other),
      Supplied = Other => L.Supplied(Other) && R.Supplied(Other)
    };
  }

  public static StringScope For(string S)
  {
    return Demand(S) & Supply(S);
  }

  public static StringScope Demand(string S)
  {
    return Unspecified with
    {
      Demanded = Other => Other(S)
    };
  }

  public static StringScope Supply(string S)
  {
    return Unspecified with
    {
      Supplied = Other => Other == S
    };
  }
}