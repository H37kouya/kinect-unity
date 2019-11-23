using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour
{
    public Text TimePanelText;
    // 説明文
    public Text ExampleText;

    // 現在時刻を更新する間隔
    public float GetTimeDiff = 10.0f;
    public float ModeChangeDiff = 5.0f;

    public Light DirectionalLight;
    public Light PointLight;
    public Light PointLight2;

    void Start()
    {
        StartCoroutine("ModeChange");
        StartCoroutine("TimeOn");
    }

    IEnumerator ModeChange()
    {
        while (true)
        {
            int nowTime = getTimeInt("m");
            ModeChangeDiff = 30 - nowTime;

            if (0 <= nowTime && nowTime < 30)
            {
                Gamemode1set();

                DataCenter.GameMode = 1;
            }

            if (30 <= nowTime)
            {
                Gamemode2set();

                DataCenter.GameMode = 2;
            }

            yield return new WaitForSeconds(ModeChangeDiff);
        }
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
    void Gamemode1set()
    {
        DirectionalLight.intensity = 0;
        PointLight.intensity = 6;
        PointLight2.intensity = 6;
        SetText("今は両手を横に動かしてみよう！");

    }
    void Gamemode2set()
    {
        DirectionalLight.intensity = 1;
        PointLight.intensity = 0;
        PointLight2.intensity = 0;
        SetText("今は左手を上下に動かしてみよう！腰が基準だよ");
    }

    void SetText(string str)
    {
        ExampleText.text = str;
    }
}
