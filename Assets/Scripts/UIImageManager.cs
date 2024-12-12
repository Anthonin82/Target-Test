using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageManager : MonoBehaviour
{
    public Image[] UIimages;    
    public int TargetRestanteIndex = 3;
    public static UIImageManager Instance;

    public void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("zglzef");        
        }
        Instance = this;
    }
    public void UIUpdater()
    {
        UIimages[TargetRestanteIndex-1].enabled = false;
        TargetRestanteIndex--;
    }
}
