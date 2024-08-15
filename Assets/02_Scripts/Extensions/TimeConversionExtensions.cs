public static class TimeConversionExtensions
{
    public static int GetSeconds(this CustomerData data) => FromValues(data.Minutes, data.Seconds);
    public static int GetSeconds(this LevelData data) => FromValues(data.CloseAfterMinutes, data.CloseAfterSeconds);
    public static int FromValues(int minutes, int seconds) => minutes * 60 + seconds;
}