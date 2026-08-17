namespace TeleprompterLib.Abstractions;

public sealed class SystemConsole : IConsole
{
    public void Write(string text) => Console.Write(text);
    public void SetTitle(string title) => Console.Title = title;
    public bool KeyAvailable => Console.KeyAvailable;
    public ConsoleKeyInfo ReadKey(bool intercept) => Console.ReadKey(intercept);
}
