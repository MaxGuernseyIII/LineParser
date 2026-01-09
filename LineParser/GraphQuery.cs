using System.Text.RegularExpressions;

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

  /// <summary>
  /// Invoked when a node represents a series of sequential steps.
  /// </summary>
  /// <param name="Steps">The steps.</param>
  /// <returns>The result of the query.</returns>
  TResult QuerySequence(IEnumerable<Pattern<TScope>> Steps);

  /// <summary>
  /// Invoked when a node represents a reference to a subpattern.
  /// </summary>
  /// <param name="Demanded">The scope of the subpattern.</param>
  /// <returns>The result of the query.</returns>
  TResult QuerySubpattern(TScope Demanded);

  /// <summary>
  /// Invoked when a node represents a regex.
  /// </summary>
  /// <param name="Regex">The regex.</param>
  /// <returns>The result of the query.</returns>
  TResult QueryRegex(Regex Regex);

  /// <summary>
  /// Invoked when a node represents something other than one of the standard nodes.
  /// </summary>
  /// <param name="Other">The nonstandard node.</param>
  /// <returns>The result of the query.</returns>
  TResult QueryOther(Pattern<TScope> Other);
}