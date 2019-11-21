using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    private float time;
    public GameObject Player;
    public GameObject[] objects;
    private GameObject useobjects;
    private Quaternion[] forwardAxis;
    private Rigidbody[] rb;

    // Use this for initialization
    void Start()
    {
       
        objects = new GameObject[100];
        rb = new Rigidbody[100];
       // useobjects = new GameObject[24];
        for (int i = 0; i < 100; i++)
        {
            Vector3 PlayerPos = Player.gameObject.transform.position;
            useobjects = (GameObject)Resources.Load("MovingCreate");
            objects[i] = Instantiate(useobjects, PlayerPos, Quaternion.identity);
            objects[i].transform.Rotate(0, 0, i * 10);
            rb[i] = objects[i].GetComponent<Rigidbody>();

        }
       
        time = 0.0f;
    }

    void Update()
    {
        time += Time.deltaTime;

            
       
        for (int i=0; i < 100; i++)
        {
            
            //Vector3 PlayerPos = Player.gameObject.transform.position;
            //useobjects = (GameObject)Resources.Load("MovingCreate");
            //objects[i] = Instantiate(useobjects, PlayerPos, Quaternion.identity);
            //objects[i].transform.Rotate(0, 0, i * 10);




            //objects[i].gameObject.transform.Translate(objects[i].transform.right);//物体正面(初期位置z軸方向)から見て右に移動
        }

        StartCoroutine("CreateObj");
    }

    IEnumerator CreateObj()
    {
       
        for ( int i = 0; i < 100; i++)
        {
            if (true)
            {

                //if (rb[i].IsSleeping())
                //{
                //    Vector3 PlayerPos = Player.gameObject.transform.position;
                //    useobjects = (GameObject)Resources.Load("MovingCreate");
                //    objects[i] = Instantiate(useobjects, PlayerPos, Quaternion.identity);
                //    objects[i].transform.Rotate(0, 0, i * 10);
                //}



                objects[i].gameObject.transform.Translate(objects[i].transform.right);

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                break;
            }
        }
    }

}