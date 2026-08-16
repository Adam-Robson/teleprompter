namespace TeleprompterLib;

public class TeleprompterConfig
{
    public int DelayInMilliseconds { get; private set; } = 200;

    public void UpdateDelay(int increment)
    {
        var newDelay = Math.Min(DelayInMilliseconds + increment, 1000);
        newDelay = Math.Max(newDelay, 20);
        DelayInMilliseconds = newDelay;
    }

    public bool Done { get; private set; }

    public void SetDone()
    {
        Done = true;
    }
}
