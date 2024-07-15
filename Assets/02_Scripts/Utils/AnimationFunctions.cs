using System;
using UnityEngine;

public static class AnimationFunctions
{
    public static float Interpolate(AnimationInterpolation interpolation, float a, float b, float t) =>
        interpolation switch
        {
            AnimationInterpolation.Linear => Lerp(a, b, t),
            AnimationInterpolation.EaseInQuart => EaseInQuart(a, b, t),
            AnimationInterpolation.EaseOutQuart => EaseOutQuart(a, b, t),
            AnimationInterpolation.EaseInOutQuart => EaseInOutQuart(a, b, t),
            AnimationInterpolation.EaseInSine => EaseInSine(a, b, t),
            AnimationInterpolation.EaseOutSine => EaseOutSine(a, b, t),
            AnimationInterpolation.EaseInOutSine => EaseInOutSine(a, b, t),
            AnimationInterpolation.EaseInCubic => EaseInCubic(a, b, t),
            AnimationInterpolation.EaseOutCubic => EaseOutCubic(a, b, t),
            AnimationInterpolation.EaseInOutCubic => EaseInOutCubic(a, b, t),
            AnimationInterpolation.EaseOutQuad => EaseOutQuad(a, b, t),
            AnimationInterpolation.EaseInOutQuad => EaseInOutQuad(a, b, t),
            AnimationInterpolation.EaseInQuad => EaseInQuad(a, b, t),
            AnimationInterpolation.BounceIn => BounceIn(a, b, t),
            AnimationInterpolation.BounceOut => BounceOut(a, b, t),
            AnimationInterpolation.BounceInOut => BounceInOut(a, b, t),
            AnimationInterpolation.ElasticIn => ElasticIn(a, b, t),
            AnimationInterpolation.ElasticOut => ElasticOut(a, b, t),
            AnimationInterpolation.ElasticInOut => ElasticInOut(a, b, t),
            AnimationInterpolation.Funzies01 => Funzies01(a, b, t),
            _ => throw new ArgumentOutOfRangeException(nameof(interpolation), interpolation, null)
        };

    // f(x)= -0.5 cos(x * 3.0F * π) + 0.5
    public static float Funzies01(float a, float b, float x)
    {
        return Lerp(a, b, -0.5F * Mathf.Cos(x * 3.0F * Mathf.PI) + 0.5F);
    }

    // f(x) = 1 - cos(x * PI / 2)
    public static float EaseInSine(float a, float b, float x)
    {
        return Lerp(a, b, 1.0F - Mathf.Cos(x * Mathf.PI / 2));
    }

    // f(x) = sin(x * PI) / 2
    public static float EaseOutSine(float a, float b, float x)
    {
        return Lerp(a, b, Mathf.Sin(x * Mathf.PI) / 2.0F);
    }

    // f(x) = -0.5 (cos(x * PI) - 1)
    public static float EaseInOutSine(float a, float b, float x)
    {
        return Lerp(a, b, -0.5F * (Mathf.Cos(Mathf.PI * x) - 1.0F));
    }

    // f(x) = -(2 ^ (10 * x - 10)) * sin((x * 10 - 10.75) * 2 * PI / 3)
    public static float ElasticIn(float a, float b, float x)
    {
        return Lerp(a, b, x == 0 ? 0 : Mathf.Abs(x - 1.0F) < 0.01F ? 1.0F : Mathf.Pow(2.0F, 10F * x - 10F) * -1.0F * Mathf.Sin((x * 10F - 10.75F) * 2.0F * Mathf.PI / 3.0F));
    }
    
    // f(x) = 2 ^ (-10 * x) * sin((x * 10 - 0.75) * 2 * PI / 3)
    public static float ElasticOut(float a, float b, float x)
    {
        return Lerp(a, b, x == 0 ? 0 : Mathf.Abs(x - 1) < 0.01 ? 1.0F : Mathf.Sin(-13.0F * Mathf.PI * (x + 1.0F)) * Mathf.Pow(2, -10.0F * x) + 1.0F);
    }

    public static float ElasticInOut(float a, float b, float x)
    {
        return Lerp(a, b, x == 0 ? 0 : Mathf.Abs(x - 1) < 0.01 ? 1.0F 
            : x < 0.5F
            ? Mathf.Pow(2.0F, 20F * x - 10F) * -1.0F * Mathf.Sin((x * 20F - 11.125F) * 2.0F * Mathf.PI / 4.5F) / 2.0F
            : Mathf.Pow(2.0F, -20F * x + 10F) * Mathf.Sin((x * 20F - 11.125F) * 2.0F * Mathf.PI / 4.5F) / 2.0F + 1.0F); 
    }

    public static float BounceIn(float a, float b, float x)
    {
        return Lerp(a, b, 1 - BounceOutInternal(1 - x));
    }

    public static float BounceOut(float a, float b, float x)
    {
        return Lerp(a, b, BounceOutInternal(x));
    }

    public static float BounceOutInternal(float x)
    {
        var n = 7.5625F;
        var d = 2.75F;
        
        if (x < 1 / d)
            return n * x * x;
        if (x < 2 / d)
            return n * (x -= 1.5F / d) * x + 0.75F;
        if (x < 2.5 / d)
            return n * (x -= 2.25F / d) * x + 0.9375F;

        return n * (x -= 2.625F / d) * x + 0.984375F;
    }

    public static float BounceInOut(float a, float b, float x)
    {
        return Lerp(a, b, x < 0.5F
            ? (1 - BounceOutInternal(1 - 2 * x)) / 2.0F
            : (1 + BounceOutInternal(2 * x - 1)) / 2.0F);
    }

    // f(x) = x ^ 2
    public static float EaseInQuad(float a, float b, float x)
    {
        return Lerp(a, b, Mathf.Pow(x, 2));
    }

    // f(x) = 1 - (1 - x) ^ 2
    public static float EaseOutQuad(float a, float b, float x)
    {
        return Lerp(a, b, 1.0F - Mathf.Pow(1.0F - x, 2));
    }

    // 0 - 0.5 | f(x) = 2 * x ^ 2
    // 0.5 - 1 | f(x) = 1 - 0.5 * (-2 * x + 2) ^ 2
    public static float EaseInOutQuad(float a, float b, float x)
    {
        return Lerp(a, b, x < 0.5
            ? 2.0F * Mathf.Pow(x, 2)
            : 1.0F - 0.5F * Mathf.Pow(-2.0F * x + 2.0F, 2));
    }

    // f(x) = x ^ 3
    public static float EaseInCubic(float a, float b, float x)
    {
        return Lerp(a, b, Mathf.Pow(x, 3));
    }

    // f(x) = 1 - (1 - x) ^ 3
    public static float EaseOutCubic(float a, float b, float x)
    {
        return Lerp(a, b, 1.0F - Mathf.Pow(1.0F - x, 3));
    }

    // 0 - 0.5 | f(x) = 2 * x ^ 2
    // 0.5 - 1 | f(x) = 1 - 0.5 * (-2 * x + 2) ^ 2
    public static float EaseInOutCubic(float a, float b, float x)
    {
        return Lerp(a, b, x < 0.5
            ? 2.0F * x * x
            : 1.0F - 0.5F * Mathf.Pow(-2.0F * x + 2.0F, 2));
    }

    // f(x) = x ^ 4
    public static float EaseInQuart(float a, float b, float x)
    {
        return Lerp(a, b, Mathf.Pow(x, 4));
    }

    // f(x) = 1 - (1 - x) ^ 4
    public static float EaseOutQuart(float a, float b, float x)
    {
        return Lerp(a, b, 1.0F - Mathf.Pow(1.0F - x, 4));
    }

    // 0 - 0.5 | f(x) = x ^ 4
    // 0.5 - 1 | f(x) = 1 - (1 - x) ^ 4
    public static float EaseInOutQuart(float a, float b, float x)
    {
        return Lerp(a, b, x < 0.5
            ? 8 * Mathf.Pow(x, 4)
            : 1 - Mathf.Pow(-2 * x + 2, 4) / 2);
    }

    // f(x) = x
    // Interpolate between A and B
    // Example: a = 0; b = 1; x = 0.5; r = 0.5;
    // Example: a = 20; b = 40; x = 0.75; r = 35; 
    public static float Lerp(float a, float b, float x)
    {
        return (1 - x) * a + x * b;
    }
}