using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static T[] Shuffle<T>(T[] array, System.Random randy)
    {
        for(int i = 0; i < array.Length; i++)
        {
            int randomIndex = randy.Next(i, array.Length);

            T tempItem = array[randomIndex];

            array[randomIndex] = array[i];
            array[i] = tempItem;
   
        }

        return array;
    }
}
