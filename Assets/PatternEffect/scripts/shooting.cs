using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooting : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject bullet;
    public Transform player;
    public float speed = 1000;
    private float nowtime = 0.0f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        nowtime += Time.deltaTime;
        // StartCoroutine("CreateObj");
        shoot(nowtime);

    }

    IEnumerator CreateObj()
    {

        for (int i = 0; i < 100; i++)
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



                GameObject bullets = Instantiate(bullet) as GameObject;
                Vector3 force;
                force = player.gameObject.transform.forward * speed;
                bullets.GetComponent<Rigidbody>().AddForce(force);
                bullets.transform.position = player.position;

                yield return new WaitForSeconds(0.1f);
            }
            else
            {
                break;
            }
        }
    }

    void shoot(float time)
    {
        GameObject bullets = Instantiate(bullet) as GameObject;
        bullets.transform.Rotate(0, 0, time);
        Vector3 force;
        force = bullets.gameObject.transform.forward * speed;
        bullets.GetComponent<Rigidbody>().AddForce(force);
        bullets.transform.position = player.position;
    }
}
