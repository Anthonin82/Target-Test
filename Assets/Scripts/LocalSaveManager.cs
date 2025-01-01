using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.VisualScripting;
using UnityEngine;

public class LocalSaveManager : MonoBehaviour
{
    //load save logic :
    //game will always close well because fuck off

    //load le file at launch
    //save when quit



    public LevelsDatabase LevelsDatabase;

    
    public static Dictionary<string, object> localSaveData = new();
    static List<string> dataKeys = new() { "username" };

    private void Awake()
    {
        for(int lvlIndex = 0; lvlIndex < LevelsDatabase.levelsCount; lvlIndex++)
        {
            dataKeys.Add("level" + lvlIndex + "_timesCount");
        }
    }

    

    public void ExtractDataFromSaveFile(string saveJsonString)
    {
        Dictionary<string, object> deserializedData = JsonConvert.DeserializeObject<Dictionary<string, object>>(saveJsonString);
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

    [ContextMenu("save data")]
    public async Task WriteSaveDataOnCloud()
    {

        Dictionary<string, object> data = new Dictionary<string, object>
        {
            { "Name", "Player1" },
            { "Health", 100 },
            { "IsAlive", true },
            { "IsDead", 1.337f },
            { "Position", "hell was made for people like you" }
        };

        string json = JsonConvert.SerializeObject(data, Formatting.Indented);
        byte[] file = Encoding.UTF8.GetBytes(json);
        await CloudSaveService.Instance.Files.Player.SaveAsync("save", file);
        Debug.Log("data written");

    }


}
