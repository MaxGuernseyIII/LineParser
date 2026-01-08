namespace ScopeSelection;

public interface MatchScope<in Scope>
  where Scope : MatchScope<Scope>
{
  public bool IsSatisfiedBy(Scope Other);
}