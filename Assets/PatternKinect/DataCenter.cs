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

    // 使う関節の総数
    public static int JointNumber;

    // 関節を感知したかどうか
    public static bool[] IsDetected;

    // 待機画面起動中かどうか
    public static bool WaitingDisplay = true;
    // web 送信してほしいかどうか
    public static bool WebSender = false;
    // 一つでもいいので、関節が動いたかどうか
    public static bool IsAllDetected()
    {
        return IsDetected[0] || IsDetected[1];
    }
}
