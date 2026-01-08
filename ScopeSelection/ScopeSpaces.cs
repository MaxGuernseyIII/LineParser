namespace ScopeSelection;

public static class ScopeSpaces
{
  public static ScopeSpace<NullScope> Null { get; } = new NullScope.Space();

  public static CompositeScope<TLeft, TRight>.Space Composite<TLeft, TRight>(ScopeSpace<TLeft> Left,
    ScopeSpace<TRight> Right)
    where TLeft : Scope<TLeft>
    where TRight : Scope<TRight>
  {
    return new(Left, Right);
  }

  public static SupplyAndDemandScope<Token>.Space SupplyAndDemand<Token>()
  {
    return new();
  }
}