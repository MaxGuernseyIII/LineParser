# LineParser

A simple, extensible library for parsing small snippets of text that supports recursion without the need for a rigid grammar definition.

## Motivation

Honestly, this is motivated by an almost singular purpose: The desire to use cucumber expressions in step argument transformations in Reqnroll.

Reqnroll currently depends on the cucumber expressions package. That package is nice but coerces a certain amount of Primitive Obsession by making its public API so heavily depend on regular expressions.

## Getting Started

Start by adding the `LineParser` and possibly the `LineParser.CucumberExpressions` package to your project.

Then create an expression:

```csharp
var ExpressionFactory = MatchScopeSpaces.Null.Get().PatternFactory();
var MeatFinder = ExpressionFactory.CreateConstant("meat");
```

An expression is a definition of one possible pattern.

After that, package the expression into a matcher:

```csharp
var Matcher = MatcherFactory.CreateFromExpressions([
  (MeatFinder, new object())
]);
```

Then use the matcher to scan a string...

```csharp
// will return []
var CheeseMatches = Matcher.ExactMatch("cheese");

// will return an enumerable with one match and no captures.
var MeatMatches = Matcher.ExactMatch("meat");
```

The return value of the `Matcher.ExactMatch` is an `IEnumerable` of `MatchWithMeaning<Meaning>`. When a pattern matches, it supplies all of possible matches that apply. Therefore: If no patterns match, the result will be empty and if there are multiple syntactically-valid interpretations, the result will have more than one entry.

## Adding Meaning

When defining the expressions in a matcher, you pass a tuple for each pattern. The first parameter of the tuple is the pattern itself. The second is an arbirtary token of meaning.

You're not stuck using `object`s as opaque cookies, either. You can control the type used to define meaning with the `Meaning` type parameter, which is the second type parameter in all the generic structures throughout `LineParser`.

For instance, if you have an interface called `IBinder` that you want to be able to use to consume a match, you can, change how you construct your `ExpressionFactory`:

```csharp
var ExpressionFactory = new ExpressionFactory<NullScope, IBinder>();
var MeatFinder = ExpressionFactory.CreateConstant("meat");
var TheMeaningOfMeat = BinderFactory.ForMeat();
```

Then pass in the meaning object when constructing your matcher:

```csharp
var Matcher = MatcherFactory.CreateFromExpressions([
  (MeatFinder, TheMeaningOfMeat)
]);
```

If the matcher finds something that matches the pattern, it will return `TheMeaningOfMeat` along with the associated match.

## Defining Scopes

The first type parameter is `Scope`. This is used to constrain the set of possible expressions in a matcher when executing a query. For instance, if you have a `Matcher` loaded with patterns for both statements and parameters, you might want to run a match that only finds statements.

(Don't worry, the statemetns will still be allowed to find subexpressions. But even saying that is a bit of a digression.)

While Scope is constrainded to implement the interface `MatchScope<Scope>`, you don't have to implement your own kind of scope. There is a class called `SupplyAndDemandScope<T>` that solves most problems. If you need multiple dimensions of scope, there's also a `CompositeScope<L, R>` that solves that problem for you.

Update your sample code to use the built in scopes. Start by adding a using...

```csharp
using StringScope = SupplyAndDemandScope<string>;
```

Then change how you construct your factory and matcher:

```csharp
var ExpressionFactory = new ExpressionFactory<StringScope, IBinder>();

// ...

var Matcher = MatcherFactory.CreateFromRegistry([
  (StringScope.Supply("food"), MeatFinder, TheMeaningOfMeat),
]);
```

To constrain how the matcher works, pass in a scope to `ExactMatch`...

```csharp
// finds the expression
var MeatMatches = Matcher.ExactMatch("meat", StringScope.Demand("food"));

// does not search for the expression because it is out of scope
var MeatMatches = Matcher.ExactMatch("meat", StringScope.Demand("airplanes"));
```

## More Sophisticated Expressions

If all you wanted to do was check equivalence, you'd use `==` or `object.Equals(L, R)`.

You can construct more sophisticated graphs:

```csharp
var ExpressionFactory = new ExpressionFactory<NullScope, string>();

var Matcher = MatcherFactory.CreateFromExpressions([
  (ExpressionFactory.CreateComposite([
      ExpressionFactory.CreateConstant("user \""),
      ExpressionFactory.CreateCapturing(
        ExpressionFactory.CreateForRegex(new("(?:[^\"]|\"\")*"))),
      ExpressionFactory.CreateConstant("\"")
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

That only barely scratches the surface of how an expression can be structured.

## Adapters

In the previous section, you saw how the `LineParser` package natively can adapt regular expressions into an expression. Another adapter that is provided in a separate package (`LineParser.CucumberExpressions`) imports the `CucumberExpressions` package's functionality into an expression graph.

Its use is quite straightforward:

```csharp
var Expression = Mapper.Map(
  "this/these is/are my/our cucumber expression(s), which we use for {Purpose} and other things.",
  Name => StringScope.Demand($"parameter:{Name}"));
```

The first parameter is the cucumber expression you want to convert. The second parameter is a mechanism to convert a parameter's name into a scope object that can be used for recursive queries.

## Recursive Matches

One of the things that makes `LineParser` so powerful is that it has a mechanism for extending a recursive grammar without having to define a complete, rigid grammar. This would probably not suffice for something as complex as a programming language but it is more than sufficient for cucumber expressions and other "little language" type structures.

You can insert a placeholder for a recursive inner pattern match quite easily...

```csharp
var SubExpression = ExpressionFactory.CreateRecursive(StringScope.Demand(ScopeOfSubPatternMatch));
```

That will execute a search using the same matcher but only with the specified scope.

The recursive matches also protect themselves from stack overflows: If a pattern reenters itself at the same point in the parsed string, it just returns nothing, forcing a search of other patterns.

## Feel Free to Reach Out

If you have any questions, just reach out to me on GitHub or LinkedIn.