using System;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource[] _musicAudioSources;
    private AudioSource[] _sfxAudioSources;
    
    public AudioManager() : base(true) { }

    public override void Awake()
    {
        base.Awake();
        InitializeAudioSources();
    }

    public void PlayMusic(AudioData audioData, bool looped = true)
    {
        var source = _musicAudioSources.FirstOrDefault(x => !x.isPlaying);
        if (source is null)
        {
            Debug.LogWarning($"[Audio Manager] Cannot play Music Track \"{audioData.name}\", because all {_musicAudioSources.Length} containers are already playing.");
            return;
        }

        source.PlayAudioData(audioData, looped);
    }

    public void PlaySFX(AudioData audioData, bool looped = false, float? delay = null)
    {
        if (!audioData) throw new ArgumentNullException(nameof(audioData));

        var source = _sfxAudioSources.FirstOrDefault(x => !x.isPlaying);
        if (source is null)
        {
            Debug.LogWarning($"[Audio Manager] Cannot play SFX \"{audioData.name}\", because all {_sfxAudioSources.Length} containers are already playing.");
            return;
        }

        source.PlayAudioData(audioData, looped, delay: delay);
    }

    public void Stop(AudioData audioData)
    {
        var source = _sfxAudioSources
            .Concat(_musicAudioSources)
            .Where(x => x.isPlaying)
            .FirstOrDefault(x => x.clip == audioData.Source);
        
        if (source is null) return;
        StopAudio(source);
    }

    public void StopAll()
    {
        foreach (var container in _musicAudioSources.Concat(_sfxAudioSources)) 
            StopAudio(container);
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        LoadMusicForScene(scene.buildIndex);
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        StopAll();
    }
    
    private void LoadMusicForScene(int sceneBuildIndex)
    {
        var audioData = AudioSettings.GetTrackBySceneIndex(sceneBuildIndex);
        if (audioData is null) return;
        
        var container = _musicAudioSources.First(x => !x.isPlaying);
        container.PlayAudioData(audioData, true);
    }

    private static void StopAudio(AudioSource source)
    {
        if (source.isPlaying) source.Stop();
        source.clip = null;
        source.volume = 1.0F;
    }

    private void InitializeAudioSources()
    {
        if (!this) return;

        var sources = GetComponents<AudioSource>()
            .Select(InitializeAudioSource)
            .ToArray();
        
        _musicAudioSources = sources
            .Where(x => x.outputAudioMixerGroup.name.ToLower() == "music")
            .ToArray();
        
        _sfxAudioSources = sources
            .Where(x => x.outputAudioMixerGroup.name.ToLower() == "sfx")
            .ToArray();
    }

    private static AudioSource InitializeAudioSource(AudioSource source)
    {
        source.clip = null;
        source.loop = false;
        source.playOnAwake = false;
        source.mute = false;
        return source;
    }
}