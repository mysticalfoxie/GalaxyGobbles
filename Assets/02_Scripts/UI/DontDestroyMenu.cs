using UnityEngine;

public class DontDestroyMenu : MonoBehaviour
{
    private static DontDestroyMenu _singleton;

    void Awake()
    {
        if (_singleton == null)
        {
            _singleton = this;
        } else if (_singleton != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
