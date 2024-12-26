using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class ExtensionClasses
{
    public static void Assert(this bool value, bool value2)
    {
        if (value != value2)
        {
            Debug.LogError("??");
        }
    }


}


