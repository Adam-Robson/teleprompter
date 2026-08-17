namespace TeleprompterLib.Abstractions;

public sealed class SystemClock : IClock
{
    public Task Delay(int milliseconds, CancellationToken cancellationToken = default) => Task.Delay(milliseconds, cancellationToken);
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}
