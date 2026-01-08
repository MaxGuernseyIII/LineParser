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
  /// <returns>The result of the query.</returns>
  TResult QueryAlternativePatterns(IEnumerable<Pattern<TScope>> Alternatives);

  /// <summary>
  /// Invoked when a node represents a constant string.
  /// </summary>
  /// <param name="Content">The constant string.</param>
  /// <returns>The result of the query.</returns>
  TResult QueryConstant(string Content);

  /// <summary>
  /// Invoked when a node represents a capture.
  /// </summary>
  /// <param name="Inner">The pattern whose output is captured.</param>
  /// <returns>The result of the query.</returns>
  TResult QueryCapturing(Pattern<TScope> Inner);

  /// <summary>
  /// Invoked when a node represents a catchall.
  /// </summary>
  /// <returns>The result of the query.</returns>
  TResult QueryAnything();
}