using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour
{
    public float rotatespeed;
    void Update()
    {
        
       
        float angle = (Input.GetAxis("Vertical") == 0 ?  Input.GetAxis("Horizontal") : Input.GetAxis("Vertical"))  * rotatespeed;


        //transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
        transform.Rotate(0, 0, angle);
    }
}