using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progressbar : MonoBehaviour
{
    public static int Progress;
    
    public static Progressbar Instance { get; set; }

    public void OnClickZeroStar()
    {
        Progress = 0;
        Debug.Log($"SetStars to {Progress}");
    }
    public void OnClickOneStar()
    {
        Progress = 1;
        Debug.Log($"SetStars to {Progress}");

    }
    
    public void OnClickTwoStar()
    {
        Progress = 2;
        Debug.Log($"SetStars to {Progress}");

    }
    
    public void OnClickThreeStar()
    {
        Progress = 3;
        Debug.Log($"SetStars to {Progress}");

    }
    
}
