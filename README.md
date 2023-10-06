# TestObjects - create tests as plain old C# objects

## What is this

A C# library to create and run tests as a console application.

## Why

This project was made to simplify test creation in C#.
Because most other test running frameworks require a lot of specific knowledge to create and run tests.

Problems with other frameworks

1. Test names
   - based on method names, so no whitespace allowed
   - no human readable strings allowed so a naming convention must be used to name tests
   - `Handle_ProductDoesNotExistAndApiProductIsNotNullWithFalseStatus_ShouldUpdateLastCheckDateAsync` --- actual name of the test in my work codebase
       - even when you can provide `[Fact(DisplayName = "some good description")]` you still need to name the method in some way
1. Based on reflections
   - "0 references" on test methods
   - you need to remember to put `[Test]` or `[Theory]` attribute
   - generating mutliple tests (so that they are counted as passed/failed separately) dynamically is difficult
1. Poor typesafety
   - `MemberData` is not typesafe, there are `object[]` passed around and you need to make sure those are the same types you expect in your test methods.
   - return types on test methods are pointless, they have to be ignored
1. Steep learning curve
   - very simple things are easy (e.g. running one test case that does not depend on anything else)
   - even slightly more complex things are very difficult (e.g. generating a `(input, expected)` pair programatically)
   - documentation needed for things like running something before or after each test
   - complex "fixtures", lifetime handling

All of those are caused because a test is not treated like a value or an **object**.
With this simple mindset, you can compose your tests, create them dynamically, filter on specific conditions, and so on using your C# knowledge.

## Features

Referring to problems in the previous section

1. Test names are just `string`s, passed as the first parameter to `new Test`
1. Plain C# objects (actually `record`s), so if the test is unused, your IDE can help you spot that.
   You can create your own abstraction on top of that.
1. As typesafe as C# is. No implicit convertions, no `object[]`, no `dynamic` or other means of circumventing C#'s typesafety.
1. Nothing is done implicitly. After creating as many tests as you want just run them with `TestRunner.Run(tests)` and you're good to go.

## Installation

<!-- TODO: nuget package -->
<!-- TODO: `dotnet new` template -->

```console
dotnet new testobj -o MyTests
```

## Usage

<!-- TODO: usage examples -->

Example using `FluentAssertions` package and top-level statements and implicit `Main` in `Program.cs`

```cs
// TestObjects.Tests/Program.cs

﻿using TestObjects;
using FluentAssertions;

return await TestRunner.RunTests(
    new Test(
        "dupa is not empty",
        () =>
        {
            var input = "dupa";
            input.Should().NotBeEmpty();
        }
    ),
    new Test(
        "failing test",
        () =>
        {
            false.Should().BeTrue();
        }
    ),
    new Test(
        "task as test function",
        async () =>
        {
            await Task.CompletedTask;
        }
    ),
    new Test("test throwing an exception", () => throw new Exception("example exception")),
    FunctionReturningTest()
);

static Test FunctionReturningTest()
{
    return new Test("hello", () => { });
}


```

## Goals

- [ ] IDE/framework integration so that tests can be viewed in the test explorer or something similar

## Non goals

<!-- TODO: -->
