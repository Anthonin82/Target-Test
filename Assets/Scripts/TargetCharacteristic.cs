using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class TargetCharacteristic : MonoBehaviour
{
    public float CompteurTarget;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            
            Destroy(gameObject);            
        }
        
    }
    
}
