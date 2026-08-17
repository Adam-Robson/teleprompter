using TeleprompterLib.Abstractions;

namespace TeleprompterLib;

public sealed class TeleprompterRunner : ITeleprompterRunner
{
    private const int PollingIntervalMilliseconds = 15;

    private readonly IConsole _console;
    private readonly IClock _clock;

    public TeleprompterRunner(IConsole console, IClock clock)
    {
        _console = console;
        _clock = clock;
    }

    public async Task<TeleprompterRunResult> RunAsync(string file, CancellationToken cancellationToken = default)
    {
        var config = new TeleprompterConfig();
        var stats = new TeleprompterStats(_clock);
        using var internalCts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        var displayTask = ShowTeleprompter(file, config, stats, internalCts.Token);
        var inputTask = GetInput(config, stats, internalCts.Token);

        await Task.WhenAny(displayTask, inputTask);
        internalCts.Cancel();

        try
        {
            await Task.WhenAll(displayTask, inputTask);
        }
        catch (OperationCanceledException)
        {
            // Expected: whichever loop was still running observed cancellation and stopped cleanly.
        }

        return new TeleprompterRunResult(stats.WordsDisplayed, stats.WordsPerMinute);
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

    private async Task ShowTeleprompter(string file, TeleprompterConfig config, TeleprompterStats stats, CancellationToken cancellationToken)
    {
        foreach (var word in ReadFrom(file))
        {
            while (config.Paused)
            {
                await Task.Delay(PollingIntervalMilliseconds, cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();

            _console.Write(word);
            if (word != Environment.NewLine)
            {
                stats.RecordWordDisplayed();
            }

            await _clock.Delay(config.DelayInMilliseconds, cancellationToken);
        }

        config.SetDone();
    }

    private async Task GetInput(TeleprompterConfig config, TeleprompterStats stats, CancellationToken cancellationToken)
    {
        while (!config.Done && !cancellationToken.IsCancellationRequested)
        {
            if (_console.KeyAvailable)
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
                else if (key.Key == ConsoleKey.Spacebar)
                {
                    config.TogglePause();
                    if (config.Paused)
                    {
                        stats.Pause();
                    }
                    else
                    {
                        stats.Resume();
                    }
                }
                else if (key.Key == ConsoleKey.X)
                {
                    config.SetDone();
                }
            }
            else
            {
                await Task.Delay(PollingIntervalMilliseconds, cancellationToken);
            }
        }
    }
}
