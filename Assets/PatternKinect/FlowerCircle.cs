using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerCircle : MonoBehaviour
{
    /**
     * 時間に関する変数
     */
    // もう一度、実行するまでの時間
    public float AllWaitTime = 1.0f;

    // このコルーチンの処理を待たせる時間
    public float WaitTime = 0.5f;

    public float WaitObjTime = 0.05f;

    // オブジェクトを削除する時間
    public float ObjDeleteTime;

    // 子供のオブジェクトを削除する時間
    public float ChildObjDeleteTime;

    /**
     * オブジェクトとそれに付随する変数
     */
    // 座標の中心の object
    public GameObject BaseObj;

    // copy する object
    private GameObject CloneObj;

    // 周りに生成するオブジェクト
    public GameObject[] Objects;

    // 周りに生成するオブジェクト (その子供)
    public GameObject[] ChildObjects;

    // 円の半径
    public int CircleRadius = 2;

    // 周りに生成するオブジェクト数
    public int CircleObjMax = 12;

    /**
     * その他
     */
    // 関節の番号 (自分で振る)
    public int JointNumber = 0;

    private Rigidbody[] rb;


    // Use this for initialization
    void Start()
    {
        Objects = new GameObject[50];
        rb = new Rigidbody[50];
        ChildObjects = new GameObject[50];
        // オブジェクトを削除する時間
        ObjDeleteTime = WaitTime * 11f;
        ChildObjDeleteTime = WaitTime * 10.05f;

        // circle 生成処理
        StartCoroutine("OnCreateCircle");
    }

    IEnumerator OnCreateCircle()
    {
        while (true)
        {
            if (DataCenter.IsDetected[JointNumber] && DataCenter.GameMode == 1)
            {
                // プレイヤーの座標取得 (更新)
                Vector3 _basePos = BaseObj.gameObject.transform.position;
                // 周りのオブジェクトを生成
                for (int circleObjIdx = 0; circleObjIdx < CircleObjMax; circleObjIdx++)
                {
                    CloneObj = (GameObject)Resources.Load("MovingCreate");

                    // 正規化されたベクトル
                    Vector3 _objVec = new Vector3(
                        CircleX(circleObjIdx, CircleObjMax, CircleRadius),
                        CircleY(circleObjIdx, CircleObjMax, CircleRadius),
                        0
                    );

                    // 周りの円の位置を計算(1回目)
                    Vector3 _objPos = _basePos + _objVec;
                    // オブジェクトを生成
                    Objects[circleObjIdx] = Instantiate(CloneObj, _objPos, Quaternion.identity);

                    //rigidbody取得
                    Rigidbody rb = Objects[circleObjIdx].GetComponent<Rigidbody>();
                    //オブジェクトに放射状に力を加える
                    rb.AddForce(_objVec * 100);

                    // 作ったオブジェクトを一定時間後に消す
                    Destroy(Objects[circleObjIdx], ObjDeleteTime);

                    // fade outを行う
                    StartCoroutine("FadeOut", Objects[circleObjIdx]);

                }

                yield return new WaitForSeconds(WaitObjTime);

                // 一回目に生成した円中心に再度描写
                for (int circleObjIdx = 0; circleObjIdx < CircleObjMax; circleObjIdx++)
                {
                    for (int circleChildObjIdx = 0; circleChildObjIdx < CircleObjMax; circleChildObjIdx++)
                    {
                        // 正規化されたベクトル
                        Vector3 _objChildVec = new Vector3(
                            CirclechildX(circleChildObjIdx, CircleObjMax, CircleRadius),
                            CirclechildY(circleChildObjIdx, CircleObjMax, CircleRadius),
                            0
                        );

                        // 周りの円の位置を計算
                        Vector3 objChildPos = Objects[circleObjIdx].gameObject.transform.position + _objChildVec;

                        // オブジェクトを生成
                        ChildObjects[circleChildObjIdx] = Instantiate(CloneObj, objChildPos, Quaternion.identity);
                        //rigidbody取得
                        Rigidbody rbc = ChildObjects[circleChildObjIdx].GetComponent<Rigidbody>();

                        //オブジェクトに放射状に力を加える
                        rbc.AddForce(_objChildVec * 100);

                        // ChildeObjを消去を予約
                        Destroy(ChildObjects[circleChildObjIdx], ChildObjDeleteTime);
                        // fade outを行う
                        StartCoroutine("FadeOut", ChildObjects[circleChildObjIdx]);

                        yield return new WaitForSeconds(WaitObjTime / CircleObjMax);
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

    float CircleX(int circleObjNum, int circleObjMax, int radius)
    {
        float _angle = circleObjNum * 360 / circleObjMax;
        float _x = Mathf.Sin(_angle * (Mathf.PI / 180));
        return _x * radius;
    }

    float CircleY(int circleObjNum, int circleObjMax, int radius)
    {
        float _angle = circleObjNum * 360 / circleObjMax;
        float _y = Mathf.Cos(_angle * (Mathf.PI / 180));
        return _y * radius;
    }

    float CirclechildX(int circleObjNum, int circleObjMax, int radius)
    {
        float _angle = circleObjNum * 360 / circleObjMax + 360 / circleObjMax / 2;
        float _x = Mathf.Sin(_angle * (Mathf.PI / 180));
        return _x * radius;
    }

    float CirclechildY(int circleObjNum, int circleObjMax, int radius)
    {
        float _angle = circleObjNum * 360 / circleObjMax + 360 / circleObjMax / 2;
        float _y = Mathf.Cos(_angle * (Mathf.PI / 180));
        return _y * radius;
    }
}
