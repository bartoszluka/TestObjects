using TestObjects;
using static TestObjects.TestObjects;
using FluentAssertions;

await RunTests(
    Test(
        "test name",
        () =>
        {
            var input = "dupa";
            input.Should().NotBeEmpty();
        }
    )
);
