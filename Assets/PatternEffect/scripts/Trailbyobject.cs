﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trailbyobject : MonoBehaviour

{
    public GameObject obj;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = obj.gameObject.transform.position;
        
    }
}
