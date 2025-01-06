using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCompteurTarget : MonoBehaviour
{
    public int NBTargetRestant = 3;
    public static PlayerCompteurTarget inst;

    private void Awake()
    {
        (inst == null).Assert(true);
        inst = this;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Target"))
        {
            
            NBTargetRestant -= 1;
            Destroy(other.gameObject);
            LevelUIManager.Instance.UpdateTargetsUI();

            if(NBTargetRestant == 0)
            {
                GameManager.inst.OnLevelWin();
            }

        }
    }

}
