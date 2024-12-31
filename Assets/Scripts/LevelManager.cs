using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static LevelManager inst;
    public float[] meilleursScores;
    public TextMeshProUGUI[] scoresText;
    
   

    private void Awake()
    {
        if (inst != null)
        {
            Debug.LogError("2levelmanager");
        }
        inst = this;
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

    [ContextMenu("Reset Scores")]
    public void ResetScores()
    {
        for (int i = 0; i < meilleursScores.Length; i++)
        {
            PlayerPrefs.SetFloat("HighScore" + i, 99999);
        }
    }


    public void LevelSelection(int levelIndex)
    {
        SceneManager.LoadScene(registeredLevelNames[levelIndex]);
    }

    public string[] registeredLevelNames;


}
