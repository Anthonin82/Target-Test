using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public float ValeurTimer = 0f;
    public CompteurTarget compteurTarget;
    public bool Jeufini;

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

        }
        else if (!Jeufini)
        {
            ValeurTimer = Time.time;
            Jeufini = true;
        }

    }
    
    
}
