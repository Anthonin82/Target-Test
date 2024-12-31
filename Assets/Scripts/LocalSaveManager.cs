using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.CloudSave;
using Unity.VisualScripting;
using UnityEngine;

public class LocalSaveManager : MonoBehaviour
{
    //a user is defined by their player id.
    //pseudo
    //foreach level :
    //  the number of times registered (max 10)
    //  the top 10 times.

    object test = 3;
    object tzest = "a";



    public static Dictionary<string, object> localSaveData = new();

    private async void Start()
    {
        Debug.Log(3 + test.ConvertTo<int>());
        Debug.Log(3 + tzest.ConvertTo<string>());
        //await InitializeLocalSaveFromDatabase();
    }

    [ContextMenu("initi")]
    public async Task InitializeLocalSaveFromDatabase()
    {
        
        var playerData = await CloudSaveService.Instance.Data.Player.LoadAsync(new HashSet<string> { "firstKeyName", "secondKeyName" });
        if (playerData.TryGetValue("firstKeyName", out var keyName))
        {
            Debug.Log($"keyName: {keyName.Value.GetAs<string>()}");
        }
        if (playerData.TryGetValue("secondKeyName", out var keyName2))
        {
            Debug.Log($"keyName2: {keyName2.Value.GetAs<string>()}");
        }

    }


}
