using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _musicAudioSource;
    private AudioSourceContainer[] _sfxAudioSources;
    
    public AudioManager() : base(true) { }

    public override void Awake()
    {
        base.Awake();
        InitializeAudioSource();
    }

    public void Play(AudioData audioData)
    {
        if (!audioData) throw new ArgumentNullException(nameof(audioData));

        var audioSource = _sfxAudioSources.FirstOrDefault(x => !x.Playing);
        if (audioSource is null) return;

        audioSource.Playing = true;
        audioSource.Source.PlayAudioData(audioData);

        StartCoroutine(CheckForCompleteState(audioSource));
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        LoadMusicForScene(scene.buildIndex);
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        StopMusicFromScene();
    }

    private static IEnumerator CheckForCompleteState(AudioSourceContainer container)
    {
        yield return new WaitForSeconds(container.Source.clip.length);
        yield return new WaitUntil(() => !container.Source.isPlaying);
        container.Playing = false;
    }
    
    private void LoadMusicForScene(int sceneBuildIndex)
    {
        var audioData = GameSettings.GetTrackBySceneIndex(sceneBuildIndex);
        if (audioData is null) return;
        _musicAudioSource.PlayAudioData(audioData, true);
    }

    private void StopMusicFromScene()
    {
        if (_musicAudioSource.isPlaying) _musicAudioSource.Stop();
        _musicAudioSource.clip = null;
        _musicAudioSource.volume = 1.0F;
    }
    
    private void InitializeAudioSource()
    {
        if (!this) return;
        var sources = GetComponents<AudioSource>();
        _musicAudioSource = sources.First(x => x.outputAudioMixerGroup.name.ToLower() == "music");
        _musicAudioSource.clip = null;
        _musicAudioSource.loop = true;
        _musicAudioSource.playOnAwake = false;
        _musicAudioSource.mute = false;
        
        _sfxAudioSources = sources
            .Where(x => x.outputAudioMixerGroup.name.ToLower() == "sfx")
            .Select(x => new AudioSourceContainer(x))
            .ToArray();
    }
    
    private void OnDrawGizmos() => InitializeAudioSource();
}