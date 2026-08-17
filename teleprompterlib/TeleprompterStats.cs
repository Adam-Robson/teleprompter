using TeleprompterLib.Abstractions;

namespace TeleprompterLib;

public sealed class TeleprompterStats
{
    private readonly IClock _clock;
    private DateTimeOffset? _startedAt;
    private DateTimeOffset? _pausedAt;
    private TimeSpan _pausedDuration = TimeSpan.Zero;

    public TeleprompterStats(IClock clock)
    {
        _clock = clock;
    }

    public int WordsDisplayed { get; private set; }

    public void RecordWordDisplayed()
    {
        _startedAt ??= _clock.UtcNow;
        WordsDisplayed++;
    }

    public void Pause()
    {
        _pausedAt ??= _clock.UtcNow;
    }

    public void Resume()
    {
        if (_pausedAt is { } pausedAt)
        {
            _pausedDuration += _clock.UtcNow - pausedAt;
            _pausedAt = null;
        }
    }

    public double WordsPerMinute
    {
        get
        {
            if (_startedAt is not { } startedAt || WordsDisplayed == 0)
            {
                return 0;
            }

            var elapsed = _clock.UtcNow - startedAt - _pausedDuration;
            var minutes = elapsed.TotalMinutes;
            return minutes > 0 ? WordsDisplayed / minutes : 0;
        }
    }
}
