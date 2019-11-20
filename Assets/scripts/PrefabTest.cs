using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    private float time;

    public GameObject[] objects;
    private GameObject useobjects;
    private Quaternion[] forwardAxis;

    // Use this for initialization
    void Start()
    {
       
        objects = new GameObject[100];
       
       // useobjects = new GameObject[24];
        for (int i = 0; i < 100; i++)
        {
            useobjects= (GameObject)Resources.Load("MovingCreate");
            objects[i] = Instantiate(useobjects, new Vector3(0.0f, 0, 0.0f), Quaternion.identity);
            objects[i].transform.Rotate(new Vector3(0, 0, i*10));

        }
       
        time = 0.0f;
    }

    void Update()
    {
        time += Time.deltaTime;

       
        for (int i=0; i < 100; i++)
        {
            // objects[i].gameObject.transform.Translate(1.0f, 0.5f, 0);
           //objects[i].transform.Rotate(new Vector3(i*10, 30, 1));

            objects[i].gameObject.transform.Translate(objects[i].transform.right);
        }

    }

    IEnumerator CreateObj()
    {
        while (true)
        {
            if (true)
            {
                GameObject obj = (GameObject)Resources.Load("MovingCreate");
                Instantiate(obj, new Vector3(0.0f, time, 0.0f), Quaternion.identity);



                yield return new WaitForSeconds(0.5f);
            }
            else
            {
                break;
            }
        }
    }

}