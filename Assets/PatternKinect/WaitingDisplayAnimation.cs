using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;

public class WaitingDisplayAnimation : MonoBehaviour
{
    public Text WaitingText;

    // animation を起こす間隔
    public float AnimationDiffTime = 10.0f;

    // 一度に回転させたい角度
    public int angle = 20;

    // animation の刻み
    public int scaleLevel = 20;
    // animation の刻みを正規化
    private float scaleLevelCoord;

    void Start()
    {
        scaleLevelCoord = 1.0f / scaleLevel;
        StartCoroutine("TextAnimation");
    }

    // animation を行うコルーチン
    IEnumerator TextAnimation()
    {
        while (true)
        {
            // 待機画面が起動状態かどうか判定
            if (DataCenter.WaitingDisplay)
            {
                // 初期化系の処理
                ScaleIntialize();
                RotateIntialize();

                for (int scaleDiff = 0; scaleDiff < scaleLevel; scaleDiff++)
                {
                    ScaleChange(); // 拡大の処理
                    RotateChange(); // 回転の処理
                    yield return new WaitForSeconds(0.05f);
                }
            }

            yield return new WaitForSeconds(AnimationDiffTime);
        }
    }

    // 回転の処理のための初期化
    void RotateIntialize()
    {
        WaitingText.transform.Rotate(0, 0, -1 * angle * scaleLevel);
    }

    // 回転をなくす (0度)
    void RotateZero()
    {
        WaitingText.transform.Rotate(0, 0, -1 * WaitingText.transform.localEulerAngles.z);
    }

    // 回転の処理
    void RotateChange()
    {
        WaitingText.transform.Rotate(new Vector3(0, 0, angle));

    }

    // 拡大・縮小の処理のための初期化
    void ScaleIntialize()
    {
        // scale を一度、 0 に初期化
        WaitingText.transform.localScale = new Vector3(0, 0, 1);
    }

    // 拡大の処理
    void ScaleChange()
    {
        float scaleX = WaitingText.transform.localScale.x;
        float scaleY = WaitingText.transform.localScale.y;

        if (scaleX + scaleLevelCoord > 1.0f || scaleY + scaleLevelCoord > 1.0f)
        {
            WaitingText.transform.localScale = new Vector3(1, 1, 1);
            return;
        }

        WaitingText.transform.localScale = new Vector3(scaleX + scaleLevelCoord, scaleY + scaleLevelCoord, 1);
    }
}
