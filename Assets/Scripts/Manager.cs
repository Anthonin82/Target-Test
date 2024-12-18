using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public float ValeurTimer = 0f;
    public int lvlID;
    public bool Jeufini;
    public TextMeshProUGUI affichageTimer;
    public UIImageManager ImageManager;
    public Scene loadedScene;

    public static Manager managerInstance;
    public void Awake()
    {
       if (managerInstance != null)
        {
            Debug.LogError("Alerte");
        }
        managerInstance = this;
    }
    private void Start()
    {
        loadedScene = SceneManager.GetActiveScene();
    }
    void Update()
    {
       
        Timer();
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
    public void Timer()
    {
        if (PlayerController.instance.GetComponent<CompteurTarget>().NBTargetRestant != 0)
        {
            ValeurTimer = Time.timeSinceLevelLoad;
            affichageTimer.text = ValeurTimer.ToString("F2");

        }
        else if (!Jeufini)
        {
            ValeurTimer = Time.timeSinceLevelLoad;
            Jeufini = true;
        }

    }
  
   
}
