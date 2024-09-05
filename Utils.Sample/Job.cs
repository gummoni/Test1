using Processors;

public class Job : IExecutable
{
    readonly Action Action;
    readonly ManualResetEventSlim FinishEvent = new();
    public Exception? IsFailed { get; private set; }
    public Job(Action action)
    {
        Action = action;
    }

    public void Execute()
    {
        try
        {
            Action();
        }
        catch (Exception ex)
        {
            IsFailed = ex;
        }
        finally
        {
            FinishEvent.Set();
        }
    }
}
