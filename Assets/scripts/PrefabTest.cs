﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    private float time;

    public GameObject[] objects;
    private GameObject useobjects;

    // Use this for initialization
    void Start()
    {
        // CubeプレハブをGameObject型で取得
        // GameObject obj = (GameObject)Resources.Load("MovingCreate");
        objects = new GameObject[24];
       // useobjects = new GameObject[24];
        for (int i = 0; i < 3; i++)
        {
            objects[i]= (GameObject)Resources.Load("MovingCreate");
            objects[i] = Instantiate(objects[i], new Vector3(0.0f, 2.0f * 0.3f * i, 0.0f), Quaternion.identity);
            
        }
        // Cubeプレハブを元に、インスタンスを生成、
        //Instantiate(obj, new Vector3(5.0f, 0.0f, 0.0f), Quaternion.identity); 
        time = 0.0f;
    }

    void Update()
    {
        // CubeプレハブをGameObject型で取得
        //GameObject obj = (GameObject)Resources.Load("MovingCreate");
        //StartCoroutine("CreateObj");

        // time += Time.deltaTime;
        // if( time < 30)
        // {

        // }

       
        for (int i=0; i < 3; i++)
        {
            objects[i].gameObject.transform.Translate(0, 0.5f, 0);
        }

        //if (transform.position.y > 5)
        //{
        //    Destroy(gameObject);
        //}
    }

    //IEnumerator CreateObj()
    //{
    //    while(true)
    //    {
    //        if (true)
    //        {
    //            GameObject obj = (GameObject)Resources.Load("MovingCreate");
    //            Instantiate(obj, new Vector3(0.0f, time, 0.0f), Quaternion.identity);

    //            yield return new WaitForSeconds(0.5f);
    //        }
    //        else
    //        {
    //            break;
    //        }
    //    }
    //}

}