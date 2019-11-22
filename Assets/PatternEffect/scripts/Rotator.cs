using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour
{
    public float rotatespeed;
    public GameObject CameraAxisObject;
    void Update()
    {
        
       
        float angle = (Input.GetAxis("Vertical") == 0 ?  Input.GetAxis("Horizontal") : Input.GetAxis("Vertical"))  * rotatespeed;
        Vector3 Axis = CameraAxisObject.transform.position;

        //transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        transform.RotateAround(Axis, Vector3.forward, angle);
    }
}