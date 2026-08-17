namespace TeleprompterLib;

public interface ITeleprompterRunner
{
    Task<TeleprompterRunResult> RunAsync(string file, CancellationToken cancellationToken = default);
}
