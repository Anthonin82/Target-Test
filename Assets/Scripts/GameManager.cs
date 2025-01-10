using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float levelTimer = 0f;
    [HideInInspector] public int lvlID;
    public bool Jeufini;
    public TextMeshProUGUI affichageTimer;
    public LevelUIManager ImageManager;
    public Scene loadedScene;
    public PauseMenu pauseMenu;
    public LevelsDatabase levelsDatabase;

    public static GameManager inst;
    public void Awake()
    {
        if (inst != null)
        {
            Debug.LogError("Alerte");
        }
        inst = this;

        lvlID = Array.IndexOf(levelsDatabase.registeredLevelNames, SceneManager.GetActiveScene().name);
        (lvlID == -1).Assert(false);

    }
    private void Start()
    {
        loadedScene = SceneManager.GetActiveScene();
    }
    void Update()
    {
        if (timerStarted) //sinon on est a 0 de l initialisation
        {
            levelTimer = Time.time - timeBeginPlay;
        }
        affichageTimer.text = levelTimer.ToString("F2");
    }
    public float timeBeginPlay = float.NaN;
    bool timerStarted = false;

   

    public void OnLevelWin()
    {
        Time.timeScale = 0f;

        //if (levelTimer <= PlayerPrefs.GetFloat("HighScore" + lvlID, 99999))
        //{
        //    PlayerPrefs.SetFloat("HighScore" + lvlID, levelTimer);
        //}

        LocalSaveManager.inst.UpdateLocalSaveData(lvlID, levelTimer);

        if(LocalSaveManager.inst.saveMode == SaveMode.Online)
        {
            LocalSaveManager.inst.WriteSaveDataOnCloudParallelExec();
        }
        else
        {
            LocalSaveManager.inst.WriteSaveFileOnLocal();
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
    }

    public void OnKeyPressed()
    {
        if (!timerStarted)
        {
            timerStarted = true;
            timeBeginPlay = Time.time;
        }        
    }
  
   
}
