using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : Singleton<AudioManager>
{
    private AudioSource _audio;
    
    public AudioManager() : base(true) { }

    public override void Awake()
    {
        base.Awake();
        InitializeAudioSource();
    }

    protected override void OnSceneLoaded(Scene scene, LoadSceneMode _)
    {
        LoadMusicForScene(scene.buildIndex);
    }

    protected override void OnSceneUnloaded(Scene scene)
    {
        StopMusicFromScene();
    }
    
    private void LoadMusicForScene(int sceneBuildIndex)
    {
        var track = GameSettings.GetTrackBySceneIndex(sceneBuildIndex);
        if (track is null) return;
        _audio.clip = track.Source;
        _audio.volume = track.Volume / 100.0F;
        _audio.loop = track.Loop;
        _audio.Play(); 
    }

    private void StopMusicFromScene()
    {
        if (_audio.isPlaying) _audio.Stop();
        _audio.clip = null;
        _audio.volume = 1.0F;
    }
    
    private void InitializeAudioSource()
    {
        _audio ??= this.GetRequiredComponent<AudioSource>();
        if (!_audio.IsAssigned()) return;
        _audio.clip = null;
        _audio.loop = true;
        _audio.playOnAwake = false;
        _audio.mute = false;
    }
    
    private void OnDrawGizmos() => InitializeAudioSource();
}