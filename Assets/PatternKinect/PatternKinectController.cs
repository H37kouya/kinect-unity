using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class PatternKinectController : MonoBehaviour
{
    void Start()
    {
        StartCoroutine("DebugOn");
    }

    IEnumerator DebugOn()
    {
        while (true)
        {
            // Debug.Log("0 : " + DataCenter.IsDetected[0]);
            // Debug.Log("1 : " + DataCenter.IsDetected[1]);
            yield return new WaitForSeconds(1);
        }
    }
}
