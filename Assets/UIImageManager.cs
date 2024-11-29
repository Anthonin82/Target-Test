using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIImageManager : MonoBehaviour
{
    public Image[] UIimages;    
    public int TargetRestanteIndex = 3;
    
    public void UIUpdater()
    {
        UIimages[TargetRestanteIndex-1].enabled = false;
        TargetRestanteIndex--;
    }
}
