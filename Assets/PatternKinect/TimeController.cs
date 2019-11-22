using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public Text TimePanelText;

    // 現在時刻を更新する間隔
    public float GetTimeDiff = 10.0f;

    void Start()
    {
        StartCoroutine("TimeOn");
    }

    IEnumerator TimeOn()
    {
        while (true)
        {
            GetTimeDiff = 10;
            if (DataCenter.WaitingDisplay)
            {
                TimePanelText.text = "現在時刻: " + getTimeStr("h") + ":" + getTimeStr("m");
                GetTimeDiff = 60 - getTimeInt("s");
            }

            yield return new WaitForSeconds(GetTimeDiff);
        }
    }

    string getTimeStr(string s = "n")
    {
        if (s == "n")
        {
            return System.DateTime.Now.ToString();
        }

        if (s == "d" || s == "h")
        {
            return getTimeInt(s).ToString();
        }

        if (s == "m" || s == "s")
        {
            return getTimeInt(s).ToString("00");
        }

        return System.DateTime.Now.ToString();
    }

    int getTimeInt(string s = "s")
    {
        if (s == "d")
        {
            return System.DateTime.Now.Day;
        }

        if (s == "h")
        {
            return System.DateTime.Now.Hour;
        }

        if (s == "m")
        {
            return System.DateTime.Now.Minute;
        }

        if (s == "s")
        {
            return System.DateTime.Now.Second;
        }

        return System.DateTime.Now.Second;
    }
}
