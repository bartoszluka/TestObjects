namespace TestObjects;

public static class TestObjects
{
    public static TestObject Test(string v, Action action)
    {
        return action;
    }

    public static async Task RunTests(params TestObject[] tests)
    {
        foreach (var test in tests)
        {
            await test.InnerTask;
        }
    }
}

public class TestObject
{
    public Task InnerTask => task;
    private readonly Task task;

    private TestObject(Action action)
    {
        task = Task.Run(action);
    }

    private TestObject(Task task)
    {
        this.task = task;
    }

    public static implicit operator TestObject(Action action)
    {
        return new(action);
    }

    public static implicit operator TestObject(Task task)
    {
        return new(task);
    }
}
