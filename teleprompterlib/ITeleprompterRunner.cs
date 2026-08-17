namespace TeleprompterLib;

public interface ITeleprompterRunner
{
    Task RunAsync(string file, CancellationToken cancellationToken = default);
}
