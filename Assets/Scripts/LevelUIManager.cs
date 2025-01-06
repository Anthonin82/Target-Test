using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum LeaderboardType { Local, Global}

public class LevelUIManager : MonoBehaviour
{
    public Image[] UIimages;    
    public static LevelUIManager Instance;

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
        UIimages[PlayerCompteurTarget.inst.NBTargetRestant].enabled = false;
    }

    

}
