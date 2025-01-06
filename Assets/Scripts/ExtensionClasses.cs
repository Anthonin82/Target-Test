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
    static bool IsSorted(this List<float> list)
    {
        for (int i = 0; i < list.Count - 1; i++)
        {
            if (list[i] > list[i + 1])
            {
                return false;
            }
        }
        return true;
    }

    public static List<float> SortedInsertTruncate(this List<float> list, float value)
    {
        (value <= list[list.Count - 1]).Assert(true); //sinonc a va retourner la meme liste et ca sert a r

        list = list.SortedInsert(value);

        list.RemoveAt(list.Count - 1);

        return list;

    }

    public static List<float> SortedInsert(this List<float> list, float value)
    {
        (list.IsSorted()).Assert(true);

        int index = 0;
        while (index < list.Count && list[index] < value)
        {
            index++;
        }

        if (index == list.Count)
        {
            list.Add(value);
        }
        else
        {
            list.Insert(index, value);
        }

        return list;

    }



}


