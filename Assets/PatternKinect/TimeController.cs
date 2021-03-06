using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TimeController : MonoBehaviour
{
    // 現在時刻を表示するパネル
    public Text TimePanelText;
    // 説明文
    public Text ExampleText;

    // 現在時刻を更新する間隔
    public float GetTimeDiff = 10.0f;
    // モードチェンジするか確認する間隔
    public float ModeChangeDiff = 5.0f;

    public Light DirectionalLight;
    public Light PointLight;
    public Light PointLight2;
    public Camera maincamera;

    public GameObject righthand; // 右手の座標
    public GameObject lefthand;  // 左手の座標
    public GameObject Movingcreate;

    void Start()
    {
        StartCoroutine("ModeChange"); // ゲームモードの変更するコルーチン
        StartCoroutine("TimeOn");     // 現在時刻を表示、更新するコルーチン
    }

    // ゲームモードを変更
    IEnumerator ModeChange()
    {
        while (true)
        {
            int nowTime = GetTimeInt("m");
            ModeChangeDiff = 30 - nowTime;

            if (0 <= nowTime && nowTime < 20)
            {
                SetGamemode1();
                DataCenter.GameMode = 1;
            }

            if (20 <= nowTime)
            {
                SetGamemode2();
                DataCenter.GameMode = 2;
            }

            yield return new WaitForSeconds(ModeChangeDiff);
        }
    }

    // 現在時刻を表示
    IEnumerator TimeOn()
    {
        GetTimeDiff = 10;

        while (true)
        {
            // if (DataCenter.WaitingDisplay) // 待機画面中のみ表示のときにifのコメントアウトを外してください
            // {
            UpdateTimePanelText(); // TimePanelTextを内容を更新
            GetTimeDiff = 60 - GetTimeInt("s");
            // }

            yield return new WaitForSeconds(GetTimeDiff);
        }
    }

    // 現在時刻 文字列用
    string getTimeStr(string s = "n")
    {
        if (s == "n")
        {
            return System.DateTime.Now.ToString();
        }

        if (s == "d" || s == "h")
        {
            return GetTimeInt(s).ToString();
        }

        if (s == "m" || s == "s")
        {
            return GetTimeInt(s).ToString("00");
        }

        return System.DateTime.Now.ToString();
    }

    // 現在時刻 int用
    int GetTimeInt(string s = "s")
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

    // カメラの設定 ゲームモード 1 用
    void SetCameraForGamemode1()
    {
        // 9レイヤーを追加
        maincamera.cullingMask |= (1 << 9);
    }

    // カメラの設定 ゲームモード 2 用
    void SetCameraForGamemode2()
    {
        // 9レイヤーを除く
        maincamera.cullingMask &= ~(1 << 9);
    }

    void SetGamemode1()
    {
        SetExampleText("今は両手を横に動かしてみよう！"); // 説明文の変更
        SetLightForGamemode1();                         // ライトの設定
        SetCameraForGamemode1();                        // カメラの設定
        SetObjectForGamemode(1);                        // オブジェクトの設定
    }

    void SetGamemode2()
    {
        SetExampleText("今は左手を上下に動かしてみよう！左肘が基準だよ"); // 説明文の変更
        SetLightForGamemode2();                                        // ライトの設定
        SetCameraForGamemode2();                                       // カメラの設定
        SetObjectForGamemode(2);                                       // オブジェクトの設定
    }

    // ゲームモード 1 用のライト
    void SetLightForGamemode1()
    {
        DirectionalLight.intensity = 0;
        PointLight.intensity = 6;
        PointLight2.intensity = 6;
    }

    // ゲームモード 2 用のライト
    void SetLightForGamemode2()
    {
        DirectionalLight.intensity = 1;
        PointLight.intensity = 0;
        PointLight2.intensity = 0;
    }

    // 説明文のテキストを更新する
    void SetExampleText(string str)
    {
        ExampleText.text = str;
    }

    // オブジェクト active を変更する
    void SetObjectForGamemode(int mode = 1)
    {
        bool _firstTrue = mode == 1; // GameMode1のときに true
        bool _secondTrue = mode == 2; // GameMode2のときに true

        righthand.SetActive(_firstTrue);
        lefthand.SetActive(_firstTrue);
        Movingcreate.SetActive(_secondTrue);
    }

    // TimePanelTextの内容を更新する
    void UpdateTimePanelText()
    {
        TimePanelText.text = "現在時刻: " + getTimeStr("h") + ":" + getTimeStr("m");
    }
}
