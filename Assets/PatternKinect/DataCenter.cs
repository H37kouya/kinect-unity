using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public static class DataCenter
{
    static DataCenter()
    {
        JointNumber = 2;

        IsDetected = new bool[JointNumber];
        for (int i = 0; i < JointNumber; i++)
        {
            IsDetected[i] = false;
        }
    }

    public static int JointNumber;
    public static bool[] IsDetected;

    public static bool IsAllDetected()
    {
        return IsDetected[0] || IsDetected[1];
    }
}
