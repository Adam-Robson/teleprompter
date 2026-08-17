using TeleprompterLib;
using TeleprompterLib.Tests.Fakes;
using Xunit;

namespace TeleprompterLib.Tests;

public class TeleprompterRunnerTests
{
    [Fact]
    public async Task RunAsync_WritesEveryWordFromFile()
    {
        var console = new FakeConsole();
        var clock = new FakeClock();
        var runner = new TeleprompterRunner(console, clock);
        var tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "hello world");

        try
        {
            await runner.RunAsync(tempFile);
        }
        finally
        {
            File.Delete(tempFile);
        }

        var fullText = string.Concat(console.WrittenText);
        Assert.Contains("hello", fullText);
        Assert.Contains("world", fullText);
    }

    [Fact]
    public async Task RunAsync_DelaysByConfiguredAmountPerWord()
    {
        var console = new FakeConsole();
        var clock = new FakeClock();
        var runner = new TeleprompterRunner(console, clock);
        var tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "one two");

        try
        {
            await runner.RunAsync(tempFile);
        }
        finally
        {
            File.Delete(tempFile);
        }

        Assert.All(clock.RecordedDelays, delay => Assert.Equal(200, delay));
    }

    [Fact]
    public async Task RunAsync_ReturnsWordCountInResult()
    {
        var console = new FakeConsole();
        var clock = new FakeClock();
        var runner = new TeleprompterRunner(console, clock);
        var tempFile = Path.GetTempFileName();
        await File.WriteAllTextAsync(tempFile, "one two three");

        TeleprompterRunResult result;
        try
        {
            result = await runner.RunAsync(tempFile);
        }
        finally
        {
            File.Delete(tempFile);
        }

        Assert.Equal(3, result.WordsDisplayed);
    }
}
