using TestObjects;
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
