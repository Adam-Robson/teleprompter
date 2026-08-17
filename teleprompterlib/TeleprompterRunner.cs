using TeleprompterLib.Abstractions;

namespace TeleprompterLib;

public sealed class TeleprompterRunner : ITeleprompterRunner
{
    private readonly IConsole _console;
    private readonly IClock _clock;

    public TeleprompterRunner(IConsole console, IClock clock)
    {
        _console = console;
        _clock = clock;
    }

    public async Task RunAsync(string file, CancellationToken cancellationToken = default)
    {
        var config = new TeleprompterConfig();
        var displayTask = ShowTeleprompter(file, config, cancellationToken);
        var inputTask = GetInput(config, cancellationToken);
        await Task.WhenAny(displayTask, inputTask);
    }

    private static IEnumerable<string> ReadFrom(string file)
    {
        string? line;
        using var reader = File.OpenText(file);
        while ((line = reader.ReadLine()) is not null)
        {
            foreach (var word in line.Split(' '))
            {
                yield return word + " ";
            }
            yield return Environment.NewLine;
        }
    }

    private async Task ShowTeleprompter(string file, TeleprompterConfig config, CancellationToken cancellationToken)
    {
        foreach (var word in ReadFrom(file))
        {
            _console.Write(word);
            await _clock.Delay(config.DelayInMilliseconds, cancellationToken);
        }
        config.SetDone();
    }

    private Task GetInput(TeleprompterConfig config, CancellationToken cancellationToken)
    {
        return Task.Run(() =>
        {
            do
            {
                var key = _console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow)
                {
                    config.UpdateDelay(-25);
                }
                else if (key.Key == ConsoleKey.DownArrow)
                {
                    config.UpdateDelay(25);
                }
                else if (key.Key == ConsoleKey.X)
                {
                    config.SetDone();
                }
            } while (!config.Done);
        }, cancellationToken);
    }
}
