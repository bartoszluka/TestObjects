namespace TestObjects;

public record Test
{
    public string TestDescription { get; }
    public Task InnerTask { get; }
    public string? SkippedBecause { get; init; }

    public Test(string testDescription, Action action)
    {
        TestDescription = testDescription;
        InnerTask = Task.Run(action);
    }

    public Test(string testDescription, Task task)
    {
        TestDescription = testDescription;
        InnerTask = task;
    }
}

internal record FailedTest(string TestDescription, string StackTrace);

public static class TestRunner
{
    private static void WriteSuccess(string text)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void WriteFailure(string text)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void WriteWarning(string text)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(text);
        Console.ResetColor();
    }

    private static void WriteNormal(string text)
    {
        Console.ResetColor();
        Console.WriteLine(text);
    }

    public static async Task<int> RunTests(params Test[] tests)
    {
        return await RunTests(tests.ToList());
    }

    public static async Task<int> RunTests(IReadOnlyList<Test> tests)
    {
        var allTests = tests.Count;
        var testOrTests = allTests == 1 ? "test" : "tests";
        var failedTestsCount = 0;
        Console.WriteLine($"Running {allTests} {testOrTests}");
        var stringBuilder = new StringBuilder();
        foreach (var (index, test) in IndexedAt1(tests))
        {
            stringBuilder.Append(test.TestDescription);
            try
            {
                if (test.SkippedBecause is string reason)
                {
                    WriteNormal(test.TestDescription);
                    WriteWarning("[SKIPPED] reason: " + reason);
                }
                else
                {
                    WriteNormal($"Running test {index}/{allTests}");
                    WriteNormal(test.TestDescription);
                    await test.InnerTask;
                    WriteSuccess("[PASSED] " + test.TestDescription);
                }
            }
            catch (System.Exception e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                WriteFailure("[FAILED] " + test.TestDescription);
                if (e.StackTrace is string stackTrace)
                {
                    WriteFailure(e.StackTrace);
                }
                failedTestsCount++;
            }
            stringBuilder.Clear();
        }

        var testSummary = new string(' ', 3) + "test summary: ";
        if (failedTestsCount == 0)
        {
            WriteSuccess($"{testSummary} all tests passed");
            return 0;
        }
        else
        {
            WriteFailure($"{testSummary} {allTests - failedTestsCount}/{allTests} tests passed");
            return 1;
        }
    }

    private static IEnumerable<(int, T)> IndexedAt1<T>(IEnumerable<T> items)
    {
        return Enumerable.Zip(Enumerable.Range(1, int.MaxValue), items);
    }
}
