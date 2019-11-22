using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Changecolor : MonoBehaviour
{
    //色が変わるタイミング(時間)を「Cube」のInspector(Duration)で指定、初期値は1.0F
    public float duration = 1.0F;
    //Hierarchyにある「Cube」を「Cube 1」にドラッグする(「Cube」のInspectorにあり)
    public GameObject cube1;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //durationの時間ごとに色が変わる
        float phi = Time.time / duration * 2 * Mathf.PI;
        float amplitude = Mathf.Cos(phi) * 0.5F + 0.5F;
        //色をRGBではなくHSVで指定
        cube1.GetComponent<Renderer>().material.color = Color.HSVToRGB(amplitude, 1, 1);

    }
}