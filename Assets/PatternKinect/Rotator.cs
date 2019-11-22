using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour
{
    public float rotatespeed;
    public GameObject CameraAxisObject;
    public GameObject Rotateparameter;

    void Start()
    {
        StartCoroutine("OnRotator");
    }

    IEnumerator OnRotator()
    {
        while (true)
        {
            if (DataCenter.IsAllDetected() && DataCenter.GameMode == 2)
            {
                // float angle = (Input.GetAxis("Vertical") == 0 ?  Input.GetAxis("Horizontal") : Input.GetAxis("Vertical"))  * rotatespeed;
                // カメラのオブジェクト
                Vector3 Axis = CameraAxisObject.transform.position;
                // 左手のオブジェクト
                Vector3 rotateparrameter = Rotateparameter.transform.position;
                if (rotateparrameter.y < 13)
                {
                    rotateparrameter.y *= -1;
                }
                float angle = rotateparrameter.y * rotatespeed;
                //transform.Rotate(new Vector3(0, 0, 45) * Time.deltaTime);
                transform.RotateAround(Axis, Vector3.forward, angle);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }
}
