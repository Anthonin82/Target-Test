using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using Unity.VisualScripting;
using UnityEngine;
using static LeaderboardType;

public class LeaderboardManager : MonoBehaviour
{
    public LeaderboardType activeLeaderboard;

    private int _levelIndex;
    public int LevelIndex
    {
        get
        {
            return _levelIndex;
        }
        set
        {
            _levelIndex = value;
            UpdateLeaderboardUI(_levelIndex);
        }
    }

    /// <summary>
    /// the rows must be set in order
    /// </summary>
    public LeaderboardRowInfo[] leaderboardRows;



    
    public void UpdateLeaderboardUI(int lvlIndex)
    {


        if(activeLeaderboard == Local)
        {
            int localScoresSavedCount = int.Parse(LocalSaveManager.LocalSaveData["level" + lvlIndex + "_timesCount"]);
            Debug.Log(LocalSaveManager.LocalSaveData["level" + 0 + "_timesCount"]);
            string json = JsonConvert.SerializeObject(LocalSaveManager.LocalSaveData, Formatting.Indented);
            Debug.Log(json);
            for (int rowIndex = 0; rowIndex < leaderboardRows.Length; rowIndex++)
            {
                LeaderboardRowInfo leaderboardRow = leaderboardRows[rowIndex];

                if (rowIndex < localScoresSavedCount)
                {
                    leaderboardRow.gameObject.SetActive(true);
                    leaderboardRow.rank.text = ""+(rowIndex+1);
                    leaderboardRow.pseudo.text = LocalSaveManager.LocalSaveData["pseudo"];
                    leaderboardRow.time.text = LocalSaveManager.LocalSaveData["level" + lvlIndex + "_time"+(rowIndex+1)];

                }
                else
                {
                    leaderboardRow.gameObject.SetActive(false);
                }
            }
        }

        else if(activeLeaderboard == Global)
        {

        }

        

        
    }

    public void OnLeaderboardLocalButton()
    {
        activeLeaderboard = Local;
        UpdateLeaderboardUI(LevelIndex);
        //on fera une variableleaderboard dans scene select et elle passera l index en arg
    }

    public void OnLeaderboardGlobalButton()
    {
        activeLeaderboard = Global;
        UpdateLeaderboardUI(LevelIndex);
    }

    
}
