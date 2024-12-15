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
    public bool Jeufini;
    public TextMeshProUGUI affichageTimer;
    public UIImageManager ImageManager;
    
    
    void Update()
    {
       
        Timer();
        if (Jeufini)
        {            
            SceneManager.LoadScene(1);           
            
        }
    }
    public void Timer()
    {
        if (PlayerController.instance.GetComponent<CompteurTarget>().NBTargetRestant != 0)
        {
            ValeurTimer = Time.time - Jouer.instance.timeInMenu;
            affichageTimer.text = ValeurTimer.ToString("F2");

        }
        else if (!Jeufini)
        {
            ValeurTimer = Time.time - Jouer.instance.timeInMenu;
            Jeufini = true;
        }

    }
  
   
}
