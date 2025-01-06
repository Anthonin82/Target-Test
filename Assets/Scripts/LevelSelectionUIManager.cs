using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionUIManager : MonoBehaviour
{
    public static LevelSelectionUIManager inst;
    public LeaderboardManager leaderboardManager;
    public float[] meilleursScores;
    public TextMeshProUGUI[] scoresText;
    public Button[] lvlButtons;
    public LevelsDatabase levelsDatabase;

    private static int _levelSelectedIndex = 0;
    public static int LevelSelectedIndex
    {
        get
        {
            return _levelSelectedIndex;
        }
        set
        {
            _levelSelectedIndex = value;
            inst.leaderboardManager.LevelIndex = value;
        }
    }
    
   

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

    private void Start()
    {
        OnLevelButton(LevelSelectedIndex);
    }

    public void Update()
    { 
        for (int i = 0; i < scoresText.Length; i++)
        {
            scoresText[i].text = meilleursScores[i].ToString("F2");
        }
    }


    public void OnRetourMainMenuButton()
    {
        SceneManager.LoadScene(0);
    }

    public void OnLevelButton(int lvlIndex)
    {
        lvlButtons[LevelSelectedIndex].GetComponent<Image>().color = Color.white;
        LevelSelectedIndex = lvlIndex;
        lvlButtons[LevelSelectedIndex].GetComponent<Image>().color = Color.red;
    }

    public void OnPlayButton()
    {
        SceneManager.LoadScene(levelsDatabase.registeredLevelNames[LevelSelectedIndex]);
    }


    [ContextMenu("Reset Scores")]
    public void ResetScores()
    {
        for (int i = 0; i < meilleursScores.Length; i++)
        {
            PlayerPrefs.SetFloat("HighScore" + i, 99999);
        }
    }


    



}
