using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class WebSender : MonoBehaviour
{
    //接続するURL
    private const string URL = "http://localhost/UnityCount/public/api/detected";

    void Start()
    {
        StartCoroutine("OnSendCheck");
    }

    IEnumerator OnSendCheck()
    {
        bool startSender = true;

        while (true)
        {
            if (DataCenter.WebSender && startSender)
            {
                startSender = false;
                StartCoroutine("OnSend", URL);

                yield return new WaitForSeconds(2.0f);
                DataCenter.WebSender = false;
                startSender = true;
            }

            yield return new WaitForSeconds(10.0f);
        }
    }

    // web 送信用のコルーチン
    IEnumerator OnSend(string url)
    {
        //POSTする情報
        WWWForm form = new WWWForm();

        //URLをPOSTで用意
        UnityWebRequest webRequest = UnityWebRequest.Post(url, form);
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
        }
    }
}
