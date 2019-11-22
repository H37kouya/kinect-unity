using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCircle : MonoBehaviour
{
    // このコルーチンの処理を待たせる時間
    public float WaitTime = 0.5f;

    public float AllWaitTime = 1.0f;

    public float WaitobjectTime = 0.05f;
    // 周りに生成するオブジェクト
    public GameObject[] objects;
    // 周りに生成するオブジェクト (その子供)
    public GameObject[] childobjects;
    // 関節の番号 (自分で振る)
    public int JointNumber = 0;

    // オブジェクトを削除する時間
    public float ObjDeleteTime;
    // 子供のオブジェクトを削除する時間
    public float childObjDeleteTime;
    // 周りに生成するオブジェクト数
    public int circleObjMax = 12;
    // 座標の中心の object
    public GameObject BaseObj;
    // copy する object
    private GameObject CloneObj;
    private Rigidbody[] rb;


    // Use this for initialization
    void Start()
    {

        objects = new GameObject[50];
        rb = new Rigidbody[50];
        childobjects = new GameObject[50];
        // オブジェクトを削除する時間
        ObjDeleteTime = WaitTime * 11f;
        childObjDeleteTime = WaitTime * 10.05f;

        // circle 生成処理
        StartCoroutine("OnCreateCircle");
    }

    IEnumerator OnCreateCircle()
    {
        //円の大きさ
        int CircleDouble = 2;

        while (true)
        {
            if (DataCenter.IsDetected[JointNumber] && DataCenter.GameMode == 1)
            {
                // プレイヤーの座標取得 (更新)
                Vector3 basePos = BaseObj.gameObject.transform.position;
                // 周りのオブジェクトを生成
                for (int circleObjIdx = 0; circleObjIdx < circleObjMax; circleObjIdx++)
                {
                    CloneObj = (GameObject)Resources.Load("MovingCreate");

                    // 正規化されたベクトル
                    Vector3 objVec = new Vector3(
                        CircleX(circleObjIdx, circleObjMax, CircleDouble),
                        CircleY(circleObjIdx, circleObjMax, CircleDouble),
                        0
                    );

                    // 周りの円の位置を計算(1回目)
                    Vector3 objPos = basePos + objVec;
                    // オブジェクトを生成
                    objects[circleObjIdx] = Instantiate(CloneObj, objPos, Quaternion.identity);

                    //rigidbody取得
                    Rigidbody rb = objects[circleObjIdx].GetComponent<Rigidbody>();
                    //オブジェクトに放射状に力を加える
                    rb.AddForce(objVec * 100);

                    // 作ったオブジェクトを一定時間後に消す
                    Destroy(objects[circleObjIdx], ObjDeleteTime);

                    // fade outを行う
                    StartCoroutine("FadeOut", objects[circleObjIdx]);

                }

                yield return new WaitForSeconds(WaitobjectTime);

                // 一回目に生成した円中心に再度描写
                for (int circleObjIdx = 0; circleObjIdx < circleObjMax; circleObjIdx++)
                {
                    for (int circleChildObjIdx = 0; circleChildObjIdx < circleObjMax; circleChildObjIdx++)
                    {
                        // 正規化されたベクトル
                        Vector3 objChildVec = new Vector3(
                            CirclechildX(circleChildObjIdx, circleObjMax, CircleDouble),
                            CirclechildY(circleChildObjIdx, circleObjMax, CircleDouble),
                            0
                        );

                        // 周りの円の位置を計算
                        Vector3 objChildPos = objects[circleObjIdx].gameObject.transform.position + objChildVec;

                        // オブジェクトを生成
                        childobjects[circleChildObjIdx] = Instantiate(CloneObj, objChildPos, Quaternion.identity);
                        //rigidbody取得
                        Rigidbody rbc = childobjects[circleChildObjIdx].GetComponent<Rigidbody>();

                        //オブジェクトに放射状に力を加える
                        rbc.AddForce(objChildVec * 100);

                        Destroy(childobjects[circleChildObjIdx], childObjDeleteTime);
                        // fade outを行う
                        StartCoroutine("FadeOut", childobjects[circleChildObjIdx]);

                        yield return new WaitForSeconds(WaitobjectTime / circleObjMax);
                    }
                }
            }

            yield return new WaitForSeconds(AllWaitTime); // n 秒処理を待つ
        }
    }

    IEnumerator FadeOut(GameObject obj)
    {
        Vector3 objVecSub = new Vector3(0.2f, 0.2f, 0);

        for (int i = 0; i < 5; i++)
        {
            obj.transform.localScale = obj.transform.localScale - objVecSub;

            yield return new WaitForSeconds(1);
        }
    }

    float CircleX(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        return x * Double;
    }

    float CircleY(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        return y * Double;
    }

    float CirclechildX(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax + 360 / circleObjMax / 2;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        return x * Double;
    }

    float CirclechildY(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax + 360 / circleObjMax / 2;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        return y * Double;
    }
}
