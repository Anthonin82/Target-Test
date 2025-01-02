using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Image[] UIimages;    
    public static UIManager Instance;

    public void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("zglzef");        
        }
        Instance = this;
    }

    /// <summary>
    /// devrait pourvoir etre appelé qd on veut, suffit de check toutes les images pas seulement la novuelle
    /// j ai la flemme de le faire
    /// </summary>
    public void UpdateTargetsUI()
    {
        UIimages[CompteurTarget.NBTargetRestant].enabled = false;
    }

    public void UpdateLeaderboardUI()
    {

    }

}
