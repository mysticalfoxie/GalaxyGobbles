using System;
using System.Collections.Generic;
using System.Linq;

public static class LinqExtensions
{
    public static T GetRandom<T>(this IEnumerable<T> values) where T : class
    {
        var array = values.ToArray();
        switch (array.Length)
        {
            case 0: return null;
            case 1: return array[0];
        }

        var random = new Random();
        var index = random.Next(0, array.Length);
        return array[index];
    }
}
