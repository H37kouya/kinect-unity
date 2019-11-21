using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WaitingDisplay : MonoBehaviour
{
    // 待機画面にすべきかどうかの確認時間
    public float WaitTime = 0.5f;

    // 待機画面のオブジェクト
    public GameObject WaitingDisplayObj;

    void Start()
    {
        StartCoroutine("WaitingDisplayOn");
    }

    IEnumerator WaitingDisplayOn()
    {
        while (true)
        {
            if (DataCenter.IsAllDetected())
            {
                WaitingDisplayObj.SetActive(false);
            }
            else
            {
                // 感知されなかったときに起動
                yield return new WaitForSeconds(WaitTime);

                Debug.Log("ON");
                WaitingDisplayObj.SetActive(true);
            }

            yield return new WaitForSeconds(WaitTime);
        }
    }
}
