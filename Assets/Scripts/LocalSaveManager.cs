using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudCode;
using Unity.Services.CloudSave;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class LocalSaveManager : MonoBehaviour
{
    //load save logic :
    //game will always close well because fuck off

    //load le file at launch
    //save when quit



    public LevelsDatabase LevelsDatabase;

    /// <summary>
    /// <para>key "pseudo" to get pseudo</para>
    /// <para>key ("level" + lvlIndex + "_timesCount") to get the number of times saved</para>
    /// <para>key ("level" + lvlIndex + "_time" + timeIndex) to get the value of a time whose idnex is between 1 and timesCount, saved to F2 format</para>
    /// </summary>
    public Dictionary<string, string> localSaveData = new();
    public Dictionary<string, string> leaderboardData = new();
    public static LocalSaveManager inst;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if(inst == null)
        {
            inst = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    public void ExtractDataFromSaveFile(string saveJsonString)
    {
        Dictionary<string, string> deserializedData = JsonConvert.DeserializeObject<Dictionary<string, string>>(saveJsonString);
        GUIUtility.systemCopyBuffer = saveJsonString;

        

        localSaveData = deserializedData;


    }

    [ContextMenu("read data")]
    public async Task LoadSaveFileFromCloud()
    {
        byte[] save = await CloudSaveService.Instance.Files.Player.LoadBytesAsync("save");
        string saveJsonString = Encoding.UTF8.GetString(save);
        ExtractDataFromSaveFile(saveJsonString);

        Debug.Log("read save from cloud");
    }


    [ContextMenu("load leaderboard")]

    public async Task LoadLeaderboardsDataFromCloud()
    {
        var customItemId = "leaderboardsData";
        var customItemData = await CloudSaveService.Instance.Data.Custom.LoadAllAsync(customItemId);

        string leaderboardJsonString = customItemData["leaderboardsDataKey"].Value.GetAs<string>();
        leaderboardData = JsonConvert.DeserializeObject<Dictionary<string, string>>(leaderboardJsonString);
        Debug.Log("leaderboard loaded");
    }

    public void UpdateLocalSaveData(int lvlIndex, float newTime)
    {
        int previousTimesCount = int.Parse(localSaveData["level" + lvlIndex + "_timesCount"]);

        if (previousTimesCount < 10)
        {
            localSaveData["level" + lvlIndex + "_timesCount"] = (previousTimesCount + 1).ToString();

            List<float> times = new List<float>();

            for (int i = 1; i <= previousTimesCount; i++)
            {
                times.Add(float.Parse(localSaveData["level" + lvlIndex + "_time" + i]));
            }
            times = times.SortedInsert(float.Parse(newTime.ToString("F2")));

            for (int i = 1; i <= previousTimesCount+1; i++)
            {
                localSaveData["level" + lvlIndex + "_time" + i] = times[i - 1].ToString();
            }

            
        }

        else if (float.Parse(localSaveData["level"+lvlIndex+"_time"+10]) > newTime) //on doit rajouter le temps
        {
            List<float> times = new List<float>();
            

            for(int i = 1; i <= 10; i++)
            {
                times.Add(float.Parse(localSaveData["level" + lvlIndex + "_time" + i]));
            }
            times = times.SortedInsertTruncate(float.Parse(newTime.ToString("F2")));

            for (int i = 1; i <= 10; i++)
            {
                localSaveData["level" + lvlIndex + "_time" + i] = times[i-1].ToString();
            }

        }
    }


    [ContextMenu("save leaderboard")]
    public async Task SaveLeaderboardsDataOnCloud()
    {
        int useless = await CloudCodeService.Instance.CallEndpointAsync<int>("SetLeaderboardData", new Dictionary<string, object>() { { "jsonValue", "jsonSTring" } });
        Debug.Log("leaderboard saved");
    }


    [ContextMenu("write save data online")]
    public async Task WriteSaveDataOnCloud()
    {
        string json = JsonConvert.SerializeObject(localSaveData, Formatting.Indented);
        byte[] file = Encoding.UTF8.GetBytes(json);
        await CloudSaveService.Instance.Files.Player.SaveAsync("save", file);
        Debug.Log("data written");
    }

    public async void WriteSaveDataOnCloudParallelExec()
    {
        await WriteSaveDataOnCloud();
    }

    [ContextMenu("reinitialize local data")]
    public void DebugReinitializeLocalSaveData()
    {
        localSaveData = new Dictionary<string, string>();
        for (int lvlIndex = 0; lvlIndex < LevelsDatabase.registeredLevelNames.Length; lvlIndex++)
        {
            localSaveData["level" + lvlIndex + "_timesCount"] = 0.ToString();
        }
        localSaveData["pseudo"] = debugPseudo;
    }
    public string debugPseudo;

    [ContextMenu("Rename local data pseudo")]
    public void RenameCurrentPseudo()
    {
        localSaveData["pseudo"] = debugPseudo;
    }


    public void InitializeLocalDataOnSignUp(string pseudo)
    {
        localSaveData = new Dictionary<string, string>();
        for(int lvlIndex = 0; lvlIndex < LevelsDatabase.registeredLevelNames.Length; lvlIndex++)
        {
            localSaveData["level" + lvlIndex + "_timesCount"] = 0.ToString();
        }
        localSaveData["pseudo"] = pseudo;
    }

    public bool OnWantsToQuit()
    {
        QuitAfterSaving();
        return false;
    }

    public async void QuitAfterSaving()
    {
        await WriteSaveDataOnCloud();
        Application.Quit();
    }





}

[InitializeOnLoad]
public static class PlayModeNotifier
{
    static PlayModeNotifier()
    {
        //EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private static void OnPlayModeStateChanged(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.ExitingPlayMode)
        {
            //Debug.Log("exit play mode");
            //LocalSaveManager.inst.QuitAfterSaving();

            //au final non car pas assez rapide
        }
    }
}
