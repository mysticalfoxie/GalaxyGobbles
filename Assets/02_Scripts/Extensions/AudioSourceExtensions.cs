using UnityEngine;

public static class AudioSourceExtensions
{
    public static void PlayAudioData(this AudioSource source, AudioData audioData, bool allowLoop = false)
    {
        source.clip = audioData.Source;
        source.volume = audioData.Volume / 100.0F;
        source.loop = allowLoop && audioData.Loop;
        source.Play();
    }
}