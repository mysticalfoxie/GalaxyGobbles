using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyMenu : MonoBehaviour
{
    private static DontDestroyMenu singleton;

    void Awake()
    {
        if (singleton == null)
        {
            singleton = this;
        } else if (singleton != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
}
