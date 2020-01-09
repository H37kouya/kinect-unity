using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class ChangeColor : MonoBehaviour
{
    // 色が変わるタイミング(時間)を「Cube」のInspector(Duration)で指定、初期値は 1.0f
    public float duration = 1.0f;
    // Hierarchyにある「Cube」を「Cube 1」にドラッグする(「Cube」のInspectorにあり)
    public GameObject cube1;
    // 色を変化させる時間
    public float TimeDiff = 10.0f;
    // グラデーション用のカラーリスト
    public Color[,] colors;

    private int _countMax = 10;

    // Use this for initialization
    void Start()
    {
        ColorIntialize();                // 色の初期化
        StartCoroutine("OnChangeColor"); // コルーチンスタート
    }

    IEnumerator OnChangeColor()
    {
        while (true)
        {
            if (DataCenter.IsAllDetected())
            {
                // 現在時刻 (秒)
                int _second = System.DateTime.Now.Second;

                // 色を RGB ではなく HSV で指定
                cube1.GetComponent<Renderer>().material.color = GetColor(_second);
                yield return new WaitForSeconds(1);
            }
            else
            {
                yield return new WaitForSeconds(TimeDiff);
            }
        }
    }

    // color の初期化
    void ColorIntialize()
    {
        colors = new Color[6, 2];

        colors[0, 0] = CreateColor("ff", "9a", "9e");
        colors[0, 1] = CreateColor("fa", "d0", "c4");

        colors[1, 0] = CreateColor("a1", "8c", "d1");
        colors[1, 1] = CreateColor("fb", "c2", "eb");

        colors[2, 0] = CreateColor("a1", "c4", "fd");
        colors[2, 1] = CreateColor("c2", "e9", "fb");

        colors[3, 0] = CreateColor("d4", "fc", "79");
        colors[3, 1] = CreateColor("96", "e6", "a1");

        colors[4, 0] = CreateColor("e0", "c3", "fc");
        colors[4, 1] = CreateColor("8e", "c5", "fc");

        colors[5, 0] = CreateColor("fa", "70", "9a");
        colors[5, 1] = CreateColor("fe", "e1", "40");
    }

    // 16 進数の色コードを c# 用に変更
    Color CreateColor(string r, string g, string b)
    {
        return new Color(
            (float)int.Parse(r, NumberStyles.AllowHexSpecifier) / 255,
            (float)int.Parse(g, NumberStyles.AllowHexSpecifier) / 255,
            (float)int.Parse(b, NumberStyles.AllowHexSpecifier) / 255
        );
    }

    // 色の生成
    Color GetColor(int second)
    {
        int _colorNum = second / _countMax;
        int _secondfirst = second % _countMax;

        // グラデーション用の色
        Color _beforeColor = colors[_colorNum, 0];
        Color _afterColor = colors[_colorNum, 1];

        // カウントの刻み
        Color _differColor = (_afterColor - _beforeColor) / (_countMax - 1);

        // before と after でグラデーションを作る。毎秒ごとに色変化。
        Color _resultColor = _beforeColor + _differColor * _secondfirst;

        return _resultColor;
    }
}
