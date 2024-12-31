using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float ValeurTimer = 0f;
    public int lvlID;
    public bool Jeufini;
    public TextMeshProUGUI affichageTimer;
    public UIImageManager ImageManager;
    public Scene loadedScene;
    public PauseMenu pauseMenu;

    public static GameManager inst;
    public void Awake()
    {
        if (inst != null)
        {
            Debug.LogError("Alerte");
        }
        inst = this;
    }
    private void Start()
    {
        loadedScene = SceneManager.GetActiveScene();
    }
    void Update()
    {

        if (timerStarted)
        {
            Timer();
        }
        
        if (Jeufini)
        {
            
            if (ValeurTimer <= PlayerPrefs.GetFloat("HighScore" + lvlID, 99999))
            {
                PlayerPrefs.SetFloat("HighScore"+lvlID, ValeurTimer);
                SceneManager.LoadScene(1);
            }
            else
            {
                SceneManager.LoadScene(1);

            }

        }
    }
    public float timeBeginPlay = float.NaN;
    bool timerStarted = false;

    /// <summary>
    /// a rename et a changer un de ces 4
    /// entre autres choses, le temps de départ n'est pas consistant suivant la frame, ca devrati commencer a fixed update
    /// </summary>
    public void Timer()
    {
        if (PlayerMovement.instance.GetComponent<CompteurTarget>().NBTargetRestant != 0)
        {
            ValeurTimer = Time.time - timeBeginPlay;
            affichageTimer.text = ValeurTimer.ToString("F2");
        }
        else if (!Jeufini)
        {
            ValeurTimer = Time.time - timeBeginPlay;
            Jeufini = true;
        }

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
