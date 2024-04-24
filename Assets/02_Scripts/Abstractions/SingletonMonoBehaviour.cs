using UnityEngine;

public class SingletonMonoBehaviour<T> : MonoBehaviour
{
    private readonly bool _dontDestroyOnLoad;
    public static T Instance { get; private set; }

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
            Destroy(gameObject);
            return;
        }

        Instance = (T)(object)this;

        if (!_dontDestroyOnLoad) return;

        DontDestroyOnLoad(this);
    }
}