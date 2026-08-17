using Microsoft.Extensions.DependencyInjection;
using TeleprompterLib;
using TeleprompterLib.Abstractions;

var services = new ServiceCollection();
services.AddSingleton<IConsole, SystemConsole>();
services.AddSingleton<IClock, SystemClock>();
services.AddTransient<ITeleprompterRunner, TeleprompterRunner>();

await using var provider = services.BuildServiceProvider();
var runner = provider.GetRequiredService<ITeleprompterRunner>();

var result = await runner.RunAsync("sampleQuotes.txt");

Console.WriteLine();
Console.WriteLine($"Words displayed: {result.WordsDisplayed}");
Console.WriteLine($"Pace: {result.WordsPerMinute:F0} wpm");
