using Processors;

public class Scheduler
{
    readonly Queue<IExecutable> Queues = new();
    readonly ManualResetEventSlim InvokeEvent = new();

    void ThreadMain()
    {
        while (true)
        {
            if (Queues.TryDequeue(out IExecutable? executable))
                executable.Execute();
            else
            {
                InvokeEvent.Wait();
                InvokeEvent.Reset();
            }
        }
    }

    protected Job Invoke(Action action)
    {
        var job = new Job(action);
        Queues.Enqueue(job);
        return job;
    }
}
