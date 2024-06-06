using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingletonMonoBehaviour<T> : MonoBehaviour where T : class
{
    private readonly bool _dontDestroyOnLoad;
    public static T Instance { get; private set; }
    
    protected bool InheritedDDoL { get; set; } 

    protected SingletonMonoBehaviour()
    {
    }

    protected SingletonMonoBehaviour(bool dontDestroyOnLoad)
    {
        _dontDestroyOnLoad = dontDestroyOnLoad;
    }

    public virtual void Awake()
    {
        if (Instance is not null)
        {
            if (Application.isEditor) return; 
            
            Destroy(gameObject);
            return;
        }

        Instance = (T)(object)this;
        
        if (Application.isEditor) return; 
        
        SceneManager.sceneUnloaded += OnSceneUnloaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.activeSceneChanged += OnSceneChanged;
        
        if (!_dontDestroyOnLoad) return;

        DontDestroyOnLoad(gameObject);
    }
    
    public void OnDestroy()
    {
        if (_dontDestroyOnLoad || InheritedDDoL) return;
        if (Instance is MonoBehaviour mono)
            Destroy(mono.gameObject);
        
        Instance = null;
    }

    protected virtual void OnSceneChanged(Scene oldScene, Scene newScene) {}
    protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) {}
    protected virtual void OnSceneUnloaded(Scene scene) {}
}