using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompteurTarget : MonoBehaviour
{
    public static int NBTargetRestant = 3;

    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            
            NBTargetRestant -= 1;
            Destroy(other.gameObject);
            UIManager.Instance.UpdateTargetsUI();

            if(NBTargetRestant == 0)
            {
                GameManager.inst.OnLevelWin();
            }

        }
    }

}
