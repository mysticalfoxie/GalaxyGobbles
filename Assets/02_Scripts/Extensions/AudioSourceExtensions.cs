using UnityEngine;

public static class AudioSourceExtensions
{
    public static void PlayAudioData(this AudioSource source, AudioData audioData, bool allowLoop = false, float? delay = null)
    {
        source.clip = audioData.Source;
        source.volume = audioData.Volume / 100.0F;
        source.loop = allowLoop && audioData.Loop;
        
        if (delay is not null)
            source.PlayDelayed(delay.Value);
        else 
            source.Play();
    }
}