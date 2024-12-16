using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static bool gamePaused;
    public GameObject pausePanel;

    public void Start()
    {
        gamePaused = false ;
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!gamePaused)
            {
                Paused();
            }            
        }
        if (Input.GetKeyDown(KeyCode.R)|| Input.GetKeyDown(KeyCode.O))
        {
            Retry();
        }


    }
    public void Retry()
    {
        Scene sceneLoaded = SceneManager.GetActiveScene();
        SceneManager.LoadScene(sceneLoaded.buildIndex);
        Time.timeScale = 1.0f;
    }
    public void RetourSelectionNiveau()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1.0f;
    }
    public void Paused() 
    {
        pausePanel.SetActive(true);        
        gamePaused = true;
        Time.timeScale = 0;
    }
}
