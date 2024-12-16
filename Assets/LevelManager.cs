using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public float[] meilleursScores;
    public TextMeshProUGUI[] scoresText;
    
   

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("2levelmanager");
        }
        Instance = this;
        for (int i = 0; i < meilleursScores.Length; i++)
        {
            meilleursScores[i] = PlayerPrefs.GetFloat("HighScore"+i, 99999);
        }

    }
    
    public void Update()
    { 
        for (int i = 0; i < scoresText.Length; i++)
        {
            scoresText[i].text = meilleursScores[i].ToString("F2");
        }
    }
    public void LevelSelection(int level)
    {
        SceneManager.LoadScene(level);
    }   

}
