using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jouer : MonoBehaviour
{
    public float timeInMenu;
    public static Jouer instance;
    public void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Alerte");
        }
        instance = this;
    }
    public void Update()
    {
        timeInMenu = Time.time;
    }
    public void LaunchGame()
    {
        SceneManager.LoadScene(1);       
    }
}
