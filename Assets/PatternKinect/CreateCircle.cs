using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;

public class CreateCircle : MonoBehaviour
{
    // このコルーチンの処理を待たせる時間
    public float WaitTime = 0.5f;

    // オブジェクトを削除する時間
    public float ObjDeleteTime;
    // 周りに生成するオブジェクト数
    public int circleObjMax = 12;
    //円の大きさ
    public int CircleRadius = 2;
    public int AddForceCorrection = 10;

    // 関節の番号 (自分で振る)
    public int JointNumber = 0;

    // 座標の中心の object
    public GameObject BaseObj;
    // copy する object
    private GameObject CloneObj;

    // 生成後の object の格納
    public GameObject[] objects;
    private Quaternion[] forwardAxis;
    // 生成した object のスクリプトの格納
    private Rigidbody[] rb;

    void Start()
    {
        ObjDeleteTime = WaitTime * 1.05f;
        objects = new GameObject[circleObjMax];
        rb = new Rigidbody[circleObjMax];

        StartCoroutine("CreateOn");
    }

    IEnumerator CreateOn()
    {
        while (true)
        {
            if (DataCenter.IsDetected[JointNumber])
            {
                // プレイヤーの座標取得 (更新)
                Vector3 basePos = BaseObj.gameObject.transform.position;

                // 周りのオブジェクトを生成
                for (int circleObjIdx = 0; circleObjIdx < circleObjMax; circleObjIdx++)
                {
                    CloneObj = (GameObject)Resources.Load("MovingCreate");

                    // 正規化されたベクトル
                    Vector3 objVec = new Vector3(
                        CircleX(circleObjIdx, circleObjMax, CircleRadius),
                        CircleY(circleObjIdx, circleObjMax, CircleRadius),
                        0
                    );

                    // 周りの円の位置を計算
                    Vector3 objPos = basePos + objVec;

                    // オブジェクトを生成
                    objects[circleObjIdx] = Instantiate(CloneObj, objPos, Quaternion.identity);

                    //rigidbody取得
                    Rigidbody rb = objects[circleObjIdx].GetComponent<Rigidbody>();
                    //オブジェクトに放射状に力を加える
                    rb.AddForce(objVec * AddForceCorrection);

                    // 作ったオブジェクトを一定時間後に消す
                    Destroy(objects[circleObjIdx], ObjDeleteTime);
                }
            }

            yield return new WaitForSeconds(WaitTime);
        }
    }

    // 円の x 座標を取得
    float CircleX(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        return x * Double;
    }

    // 円の y 座標を取得
    float CircleY(int circleObjNum, int circleObjMax, int Double)
    {
        float angle = circleObjNum * 360 / circleObjMax;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        return y * Double;
    }
}
