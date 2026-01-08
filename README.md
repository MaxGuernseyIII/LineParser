# LineParser

A simple, extensible library for parsing small snippets of text that supports recursion without the need for a rigid grammar definition.

## Motivation

Honestly, this is motivated by an almost singular purpose: The desire to use cucumber expressions in step argument transformations in Reqnroll.

Reqnroll currently depends on the cucumber expressions package. That package is nice but coerces a certain amount of Primitive Obsession by making its public API so heavily depend on regular expressions.

## Getting Started

Start by adding the `LineParser` and possibly the `LineParser.CucumberExpressions` package to your project.

Then create an a pattern:

```csharp
    var Factory = MatchScopeSpaces.Null.GetFactory();
    var MeatPattern = Factory.Constant("meat");
```

After that, package the pattern into a matcher:

```csharp
    var Matcher = Factory.Matcher([MeatPattern]);
```

Then use the matcher to scan a string...

```csharp
    Matcher.ExactMatch("meat").Count().ShouldBe(1);
    Matcher.ExactMatch("cheese").Count().ShouldBe(0);
```

The return value of the `Matcher.ExactMatch` is an `IEnumerable` of `MatchWithMeaning<Meaning>`. When a pattern matches, it supplies all of possible matches that apply. Therefore: If no patterns match, the result will be empty and if there are multiple syntactically-valid interpretations, the result will have more than one entry.

## Adding Meaning

Patterns have the ability to be associated with an arbitrary object that assigns a meaning to each pattern. You're not stuck using `object`s as opaque cookies, either.

You can control the type used to define meaning with the `Meaning` type parameter.

For instance, if you have a class called `Binder` that you want to be able to use to consume a match, change how you construct your `Matcher` to drive inference of the `Meaning` type and supply appropriate meaning objects:

```csharp
    var TheMeaningOfMeat = BinderFactory.ForMeat();
    var Matcher = Factory.Matcher([(MeatPattern, TheMeaningOfMeat)]);
```

The resulting match will carry the associated meaning.

```csharp
    Matcher.ExactMatch("meat").Single().Meaning.ShouldBeSameAs(TheMeaningOfMeat);
```

If the matcher finds something that matches the pattern, it will return `TheMeaningOfMeat` along with the associated match.

## Defining Scopes

Patterns can also be associated with scopes, which are used to constrain the set of possible expressions in a matcher when executing a query.

For instance, if you have a `Matcher` loaded with patterns for both statements and parameters, you might want to run a match that only finds statements.

(Don't worry, the statemetns will still be allowed to find subexpressions. But even saying that is a bit of a digression.)

All scopes must implement the interface `MatchScope<Scope>`, but you don't have to implement your own kind of scope. There is a class called `SupplyAndDemandScope<T>` that solves most problems. If you need multiple dimensions of scope, there's also a `CompositeScope<L, R>` that solves that problem for you.

Update your sample code to use the built in scopes. Start by adding a using a different scope space to make your factory.

```csharp
    var ScopeSpace = MatchScopeSpaces.SupplyAndDemand<string>();
    var Factory = ScopeSpace.GetFactory();
```

Then change how you construct your matcher:

```csharp
    var Matcher = Factory.Matcher([(ScopeSpace.Supply("food"), MeatPattern, TheMeaningOfMeat)]);
```

To constrain how the matcher works, pass in a scope to `ExactMatch`...

```csharp
    Matcher.ExactMatch("meat", ScopeSpace.Demand("food")).Count().ShouldBe(1);
    Matcher.ExactMatch("meat", ScopeSpace.Demand("airplanes")).Count().ShouldBe(0);
```

## More Sophisticated Expressions

If all you wanted to do was check equivalence, you'd use `==` or `object.Equals(L, R)`.

You can construct more sophisticated graphs:

```csharp
    var ExpressionFactory = MatchScopeSpaces.Null.GetFactory();
    var Matcher = ExpressionFactory.Matcher(
      [
        (ExpressionFactory.Composite([
            ExpressionFactory.Constant("user \""),
            ExpressionFactory.Capturing(
              ExpressionFactory.Regex(new("(?:[^\"]|\"\")*"))),
            ExpressionFactory.Constant("\"")
          ]),
          "username")
      ]);

    var Matches = Matcher.ExactMatch("user \"jumper9\"");

    Matches.ShouldBe([
      new()
      {
        Match = new()
        {
          Matched = "user \"jumper9\"",
          Remainder = "",
          Captured =
          [
            new()
            {
              At = 6,
              Value = "jumper9"
            }
          ]
        },
        Meaning = "username"
      }
    ]);
```

That only barely scratches the surface of how a pattern can be structured.

## Adapters

In the previous section, you saw how the `LineParser` package natively can adapt regular expressions into an pattern. Another adapter that is provided in a separate package (`LineParser.CucumberExpressions`) imports the `CucumberExpressions` package's functionality a pattern graph.

Its use is quite straightforward:

```csharp
    var Pattern = Factory.CucumberExpression(
      "this/these is/are my/our cucumber expression(s), which we use for {Purpose} and other things.",
      Name => ScopeSpace.Demand($"parameter:{Name}"));
```

The first parameter is the cucumber expression template you want to convert. The second parameter is a mechanism to convert a parameter's name into a scope object that can be used for recursive queries.

## Recursive Matches

One of the things that makes `LineParser` so powerful is that it has a mechanism for extending a recursive parsing without having to define a complete, rigid grammar. This would probably not suffice for something as complex as a programming language but it is more than sufficient for cucumber expressions and other "little language" type structures.

You can insert a placeholder for a recursive inner pattern match quite easily...

```csharp
var SubExpression = ExpressionFactory.CreateRecursive(StringScope.Demand(ScopeOfSubPatternMatch));
```

That will execute a search using the same matcher but only with the specified scope.

The recursive matches also protect themselves from stack overflows: If a pattern reenters itself at the same point in the parsed string, it just returns nothing, forcing a search of other patterns.

## Feel Free to Reach Out

If you have any questions, just reach out to me on GitHub or LinkedIn.