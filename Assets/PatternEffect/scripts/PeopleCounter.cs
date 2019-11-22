using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PeopleCounter : MonoBehaviour
{
    public Text CounterDisplay;

    public const string URL = "http://localhost/UnityCount/public/api/get-people-count";

    public bool WebSenderCheck = false;

    public float nowTime;

    public float pollingTime = 60.0f;

    void Start()
    {
        StartCoroutine("OnGetStart");
        // StartCoroutine("OnGetBytoday", URL);
    }

    IEnumerator OnGetStart()
    {
        bool startSender = true;

        while (true)
        {
            if (DataCenter.WebSender && startSender)
            {
                DataCenter.WebSender = false;
                StartCoroutine("OnGetBytoday", URL);
                startSender = false;
            }


            yield return new WaitForSeconds(10.0f);
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
            WebSenderCheck = true;
        }
        else
        {
            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
            CounterDisplay.text = "今日の来訪者数: " + webRequest.downloadHandler.text + "人";
            WebSenderCheck = true;
        }
    }

    IEnumerator OnGetByYesterday(string url)
    {
        string requestUrl = url;

        //URLをGETで用意
        UnityWebRequest webRequest = UnityWebRequest.Get(requestUrl + "?date='yesterday'");
        //UnityWebRequestにバッファをセット
        webRequest.downloadHandler = new DownloadHandlerBuffer();
        //URLに接続して結果が戻ってくるまで待機
        yield return webRequest.SendWebRequest();

        //エラーが出ていないかチェック
        if (webRequest.isNetworkError)
        {
            //通信失敗
            Debug.Log(webRequest.error);
            WebSenderCheck = true;
        }
        else
        {
            //通信成功
            Debug.Log(webRequest.downloadHandler.text);
            CounterDisplay.text = "今日の来訪者数: " + webRequest.downloadHandler.text + "人";
            WebSenderCheck = true;
        }
    }
}
