using System;

public static class TimeConverter
{
    public static uint GetSeconds(this CustomerData data) => FromValues(data.Minutes, data.Seconds);
    public static uint GetSeconds(this LevelData data) => FromValues(data.CloseAfterMinutes, data.CloseAfterSeconds);
    public static uint FromValues(uint minutes, uint seconds) => minutes * 60 + seconds;
}