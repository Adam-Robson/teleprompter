using TeleprompterLib.Abstractions;

namespace TeleprompterLib.Tests.Fakes;

public sealed class FakeClock : IClock
{
    public DateTimeOffset UtcNow { get; set; } = DateTimeOffset.UtcNow;
    public List<int> RecordedDelays { get; } = new();

    public Task Delay(int milliseconds, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        RecordedDelays.Add(milliseconds);
        UtcNow = UtcNow.AddMilliseconds(milliseconds);
        return Task.CompletedTask;
    }
}
