namespace TeleprompterLib;

public static class Teleprompter
{
    public static async Task RunAsync(string file)
    {
        var config = new TeleprompterConfig();
        var displayTask = ShowTeleprompter(file, config);
        var inputTask = GetInput(config);
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

    private static async Task ShowTeleprompter(string file, TeleprompterConfig config)
    {
        foreach (var word in ReadFrom(file))
        {
            Console.Write(word);
            await Task.Delay(config.DelayInMilliseconds);
        }
        config.SetDone();
    }

    private static async Task GetInput(TeleprompterConfig config)
    {
        await Task.Run(() =>
        {
            do
            {
                var key = Console.ReadKey(true);
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
        });
    }
}
