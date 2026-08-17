using TeleprompterLib.Abstractions;

namespace TeleprompterLib.Tests.Fakes;

public sealed class FakeConsole : IConsole
{
    public List<string> WrittenText { get; } = new();
    public string? Title { get; private set; }
    public Queue<ConsoleKeyInfo> QueuedKeys { get; } = new();

    public void Write(string text) => WrittenText.Add(text);
    public void SetTitle(string title) => Title = title;
    public bool KeyAvailable => QueuedKeys.Count > 0;

    public ConsoleKeyInfo ReadKey(bool intercept) =>
        QueuedKeys.Count > 0
            ? QueuedKeys.Dequeue()
            : new ConsoleKeyInfo(' ', ConsoleKey.Spacebar, false, false, false);
}
