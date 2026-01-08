namespace LineParser;

/// <summary>
/// A query against a graph of patterns.
/// </summary>
public interface GraphQuery<in TScope, out TResult>
  where TScope : Scope<TScope>
{
  /// <summary>
  /// Invoked when a node represents a set of parallel alternatives.
  /// </summary>
  /// <param name="Alternatives"></param>
  /// <returns></returns>
  TResult QueryAlternativePatterns(IEnumerable<Pattern<TScope>> Alternatives);
}