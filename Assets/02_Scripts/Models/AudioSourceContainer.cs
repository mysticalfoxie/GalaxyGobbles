using UnityEngine;

public class AudioSourceContainer
{
    public AudioSourceContainer(AudioSource source)
    {
        Source = source;
    }
        
    public bool Playing { get; set; }
    public AudioSource Source { get; }
}