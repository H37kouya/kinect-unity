using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabTest : MonoBehaviour
{
    private float time;
    public GameObject Player;
    public GameObject[] objects;
    public GameObject[] childobjects;
    public int circleObjMaX;
    private GameObject useobjects;
    private Quaternion[] forwardAxis;
    private Rigidbody[] rb;

    private int count;

    // Use this for initialization
    void Start()
    {

        objects = new GameObject[50];
        rb = new Rigidbody[50];
        childobjects = new GameObject[50];

        time = 0.0f;

        // circle 生成処理
        StartCoroutine("OnCreateCircle");
    }

    void Update()
    {
        time += Time.deltaTime;
    }

    IEnumerator OnCreateCircle()
    {
        // このコルーチンの処理を待たせる時間
        float WaitTime = 0.1f;
        float AllWaitTime = 1f;
        float WaitobjectTime = 0.05f;
        // オブジェクトを削除する時間
        float ObjDeleteTime = WaitTime * 11f;
        float childObjDeleteTime = WaitTime * 10.05f;
        // 周りに生成するオブジェクト数
       // int circleObjMaX = 3;
        //円の大きさ
        int CircleDouble = 2;

        // プレイヤーの座標取得
        Vector3 PlayerPos = Player.gameObject.transform.position;

        while (true)
        {
            if (PlayerPos != Player.gameObject.transform.position)
            {
                // プレイヤーの座標取得 (更新)
                PlayerPos = Player.gameObject.transform.position;

                //for (int i = 0; i < circleObjMaX; i++)
                //{
                // 周りのオブジェクトを生成
                for (int circleObjIdx = 0; circleObjIdx < circleObjMaX; circleObjIdx++)
                {
                    useobjects = (GameObject)Resources.Load("MovingCreate");

                    // 正規化されたベクトル
                    Vector3 objVec = new Vector3(
                        CircleX(circleObjIdx, circleObjMaX, CircleDouble),
                        CircleY(circleObjIdx, circleObjMaX, CircleDouble),
                        0
                    );

                    // 周りの円の位置を計算(1回目)
                    Vector3 objPos = PlayerPos + objVec; // = objectsVec

                    // オブジェクトを生成
                    objects[circleObjIdx] = Instantiate(useobjects, objPos, Quaternion.identity);
                    //rigidbody取得
                    Rigidbody rb = objects[circleObjIdx].GetComponent<Rigidbody>();

                    //オブジェクトに放射状に力を加える
                    rb.AddForce(objVec * 100);
                    Destroy(objects[circleObjIdx], ObjDeleteTime);
                    yield return new WaitForSeconds(WaitobjectTime);

                }

                yield return new WaitForSeconds(WaitTime);

                for (int circleObjIdx = 0; circleObjIdx < circleObjMaX; circleObjIdx++)
                {
                    for (int circleChildObjIdx = 0; circleChildObjIdx < circleObjMaX; circleChildObjIdx++)
                    {
                        // 正規化されたベクトル
                        Vector3 objChildVec = new Vector3(
                            CirclechildX(circleChildObjIdx, circleObjMaX, CircleDouble),
                            CirclechildY(circleChildObjIdx, circleObjMaX, CircleDouble),
                            0
                        );

                        // 周りの円の位置を計算
                        Vector3 objChildPos = objects[circleObjIdx].gameObject.transform.position + objChildVec;

                        // オブジェクトを生成
                        childobjects[circleChildObjIdx] = Instantiate(useobjects, objChildPos, Quaternion.identity);
                        //rigidbody取得
                        Rigidbody rbc = childobjects[circleChildObjIdx].GetComponent<Rigidbody>();

                        //オブジェクトに放射状に力を加える
                        rbc.AddForce(objChildVec * 100);
                        Destroy(childobjects[circleChildObjIdx], childObjDeleteTime);
                        yield return new WaitForSeconds(WaitobjectTime / circleObjMaX);

                    }
                    //Debug.Log(objects[circleObjIdx].gameObject.transform.position);

                    ////一回目に生成した円中心に再度描写



                }
            }

            yield return new WaitForSeconds(AllWaitTime); // n 秒処理を待つ
        }
    }

    float CircleX(int circleObjNum, int circleObjMaX, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMaX;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        return x * Double;

    }

    float CircleY(int circleObjNum, int circleObjMaX, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMaX;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        return y * Double;

    }


    float CirclechildX(int circleObjNum, int circleObjMaX, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMaX + 360 / circleObjMaX / 2;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        return x * Double;

    }

    float CirclechildY(int circleObjNum, int circleObjMaX, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMaX + 360 / circleObjMaX / 2;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        return y * Double;

    }
}
