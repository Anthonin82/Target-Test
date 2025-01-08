using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
public enum LeaderboardType { Local, Global}

public class LevelUIManager : MonoBehaviour
{
    public Image[] UIimages;
    public Image UIimagePrefab;
    public GameObject TargetsUI;
    public static LevelUIManager Instance;
    

    public void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError("zglzef");        
        }
        Instance = this;
    }
    public void Start()
    {
        
    }

    /// <summary>
    /// devrait pourvoir etre appelé qd on veut, suffit de check toutes les images pas seulement la novuelle
    /// j ai la flemme de le faire
    /// </summary>
    public void UpdateTargetsUI()
    {
        UIimages[GameManager.inst.NBTargetRestante].enabled = false;
    }
    public void InitialiseTargetUI()
    {
        UIimages = new Image[GameManager.inst.NBTargetMax];
        
        for (int i = 0; i < UIimages.Length; i++) 
        {
            Image nouvelleImage = Instantiate(UIimagePrefab, Vector3.zero, Quaternion.identity, TargetsUI.transform);
            UIimages[i] = nouvelleImage;
        }

    }


}
