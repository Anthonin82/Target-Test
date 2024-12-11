using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurTarget : MonoBehaviour
{
    public UIImageManager UIImageManager;
    public int NBTargetRestant = 3;

    public void Awake()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            
            NBTargetRestant -= 1;
            Destroy(other.gameObject);
            UIImageManager.UIUpdater();
        }
    }

}
