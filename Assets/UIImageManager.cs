using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageManager : MonoBehaviour
{
    public Image[] UIimages;    
    public int TargetRestanteIndex;
    
    public void UIDisabler(int Random)
    {
        TargetRestanteIndex = Random;
        UIimages[TargetRestanteIndex].gameObject.SetActive(false);

    }
}
