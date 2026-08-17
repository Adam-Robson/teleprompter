namespace TeleprompterLib.Abstractions;

public interface IConsole
{
    void Write(string text);
    void SetTitle(string title);
    bool KeyAvailable { get; }
    ConsoleKeyInfo ReadKey(bool intercept);
}
