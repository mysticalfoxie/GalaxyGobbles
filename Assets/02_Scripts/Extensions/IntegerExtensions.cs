
    public static class IntegerExtensions
    {
        public static string ToPositionString(this int value)
            => value switch
            {
                0 => "0",
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => value + "th"
            };
    }