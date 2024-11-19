using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Timer : MonoBehaviour
{
    public float ValeurTimer = 0f;
    public CompteurTarget compteurTarget;
    public bool Jeufini;

    void Update()
    {
        
        if (compteurTarget.NBTargetRestant != 0) 
        {
            ValeurTimer = Time.time;
                        
        }
        else if (!Jeufini)
        {
            ValeurTimer =Time.time;
            Jeufini = true;
        }
    }
    
    
}
