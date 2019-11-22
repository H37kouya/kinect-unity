﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Globalization;

public class Changecolor : MonoBehaviour
{
    //色が変わるタイミング(時間)を「Cube」のInspector(Duration)で指定、初期値は1.0F
    public float duration = 1.0F;
    //Hierarchyにある「Cube」を「Cube 1」にドラッグする(「Cube」のInspectorにあり)
    public GameObject cube1;
    public float TimeDiff = 10.0f;

    public Color[,] colors;
    // Use this for initialization
    void Start()
    {
        StartCoroutine("");

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

    IEnumerator OnChangeColor()
    {
        int count = 0;
        int countMax = 10;

        while (true)
        {

            if (DataCenter.IsAllDetected())
            {
                //色をRGBではなくHSVで指定
                cube1.GetComponent<Renderer>().material.color = GetColor(count, countMax);
                count++;

                if (count >= countMax)
                {
                    count = 0;
                }

            }
            else
            {
                count = 0;
            }
            yield return new WaitForSeconds(TimeDiff);
        }
    }

    Color GetColor(int count, int countMax)
    {
        int minute = System.DateTime.Now.Minute;
        int colorNum = minute % 10;

        Color BeforeColor = colors[colorNum, 0];
        Color AfterColor = colors[colorNum, 1];

        // before と after でグラデーションを作る。毎秒ごとに色変化。
        Color ResultColor = new Color();
       
        return ResultColor;
    }

    Color CreateColor(string r, string g, string b)
    {
        return new Color(
            (float)int.Parse(r, NumberStyles.AllowHexSpecifier) / 255,
            (float)int.Parse(g, NumberStyles.AllowHexSpecifier) / 255,
            (float)int.Parse(b, NumberStyles.AllowHexSpecifier) / 255
        );
    }
}