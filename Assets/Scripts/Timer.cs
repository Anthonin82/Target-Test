using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public float ValeurTimer = 0f;
    public CompteurTarget compteurTarget;
    public bool Jeufini;
    public TextMeshProUGUI affichageTimer;
    public UIImageManager ImageManager;

    void Update()
    {

        Timer();
        if (Jeufini)
        {
            Scene sceneLoaded = SceneManager.GetActiveScene();
            SceneManager.LoadScene(sceneLoaded.buildIndex + 1);
        }
    }
    public void Timer()
    {
        if (compteurTarget.NBTargetRestant != 0)
        {
            ValeurTimer = Time.time;
            affichageTimer.text = ValeurTimer.ToString("F2");

        }
        else if (!Jeufini)
        {
            ValeurTimer = Time.time;
            Jeufini = true;
        }

    }
   
}
