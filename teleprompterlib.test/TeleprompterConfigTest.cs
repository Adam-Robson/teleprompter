using TeleprompterLib;
using Xunit;

// Unit tests for the TeleprompterConfig class
// Arrange (create config) -> Act (call UpdateDelay or SetDone) -> Assert (check result)
namespace TeleprompterLib.Test;

public class TeleprompterConfigTest
{
    [Fact]
    public void UpdateDelay_StartsAt200()
    {
        var config = new TeleprompterConfig();

        Assert.Equal(200, config.DelayInMilliseconds);
    }

    [Theory]
    [InlineData(25, 225)] // one down-arrow press: slows down
    [InlineData(-25, 175)] // one up-arrow press: speedss up
    public void UpdateDelay_AppliesIncrementWithinBounds(int increment, int expected)
    {
        var config = new TeleprompterConfig();
        config.UpdateDelay(increment);
        Assert.Equal(expected, config.DelayInMilliseconds);
    }
    [Fact]
    public void UpdateDelay_ClampsAtUpperBound()
    {
        var config = new TeleprompterConfig();

        config.UpdateDelay(100_000);

        Assert.Equal(1000, config.DelayInMilliseconds);
    }

    [Fact]
    public void UpdateDelay_ClampsAtLowerBound()
    {
        var config = new TeleprompterConfig();

        config.UpdateDelay(-100_000);

        Assert.Equal(20, config.DelayInMilliseconds);
    }

    [Fact]
    public void SetDone_MakesDoneTrue()
    {
        var config = new TeleprompterConfig();

        Assert.False(config.Done);
        config.SetDone();
        Assert.True(config.Done);
    }
}
