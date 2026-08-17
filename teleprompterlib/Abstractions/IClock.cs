namespace TeleprompterLib.Abstractions;

public interface IClock
{
    Task Delay(int milliseconds, CancellationToken cancellationToken = default);
    DateTimeOffset UtcNow { get; }
}
