using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WebGetCountDisplay : MonoBehaviour
{
    public Text CounterDisplay;
    public Text CounterDisplayYesterday;

    public const string URL = "http://localhost/UnityCount/public/api/get-people-count";

    // counter display の内容を更新する間隔
    public const float pollingTime = 60.0f;
    public const float pollingTimeYesterday = 3600.0f;

    void Start()
    {
        StartCoroutine("OnGetStart");
        StartCoroutine("OnGetStartYesterday");
    }

    IEnumerator OnGetStart()
    {
        while (true)
        {
            StartCoroutine("OnGetBytoday", URL);
            yield return new WaitForSeconds(pollingTime);
        }
    }

    IEnumerator OnGetStartYesterday()
    {
        while (true)
        {
            StartCoroutine("OnGetByYesterday", URL);
            yield return new WaitForSeconds(pollingTimeYesterday);
        }
    }

    IEnumerator OnGetBytoday(string url)
    {
        string requestUrl = url;

        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl);
        //UnityWebRequestにバッファをセット
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            //通信失敗
            Debug.Log(webRequest.error);
        }
        else
        {
            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
            CounterDisplay.text = "今日の来訪者数: " + webRequest.downloadHandler.text + "人";
        }
    }

    IEnumerator OnGetByYesterday(string url)
    {
        string requestUrl = url;

        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl + "?date=yesterday");
        //UnityWebRequestにバッファをセット
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            //通信失敗
            Debug.Log(webRequest.error);
        }
        else
        {
            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
            CounterDisplayYesterday.text = "昨日の来訪者数: " + webRequest.downloadHandler.text + "人";
        }
    }
}
