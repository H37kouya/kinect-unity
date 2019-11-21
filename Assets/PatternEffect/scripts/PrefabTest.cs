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

    private int count;

    // Use this for initialization
    void Start()
    {

        objects = new GameObject[100];
        rb = new Rigidbody[100];

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
        float WaitTime = 0.5f;
        // オブジェクトを削除する時間
        float ObjDeleteTime = WaitTime * 1.05f;
        // 周りに生成するオブジェクト数
        int circleObjMaX = 12;

        // プレイヤーの座標取得
        Vector3 PlayerPos = Player.gameObject.transform.position;

        while (true)
        {
            if (PlayerPos != Player.gameObject.transform.position)
            {
                // プレイヤーの座標取得 (更新)
                PlayerPos = Player.gameObject.transform.position;

                // 周りのオブジェクトを生成
                for (int circleObjIdx = 0; circleObjIdx < circleObjMaX; circleObjIdx++)
                {
                    useobjects = (GameObject)Resources.Load("MovingCreate");

                    // 周りの円の位置を計算
                    Vector3 objectsVec = new Vector3(
                        PlayerPos.x + CircleX(circleObjIdx, circleObjMaX),
                        PlayerPos.y + CircleY(circleObjIdx, circleObjMaX),
                        PlayerPos.z
                    );

                    // オブジェクトを生成
                    objects[circleObjIdx] = Instantiate(useobjects, objectsVec, Quaternion.identity);

                    // 作ったオブジェクトを一定時間後に消す
                    Destroy(objects[circleObjIdx], ObjDeleteTime);
                }
            }

            yield return new WaitForSeconds(WaitTime); // n 秒処理を待つ
        }
    }

    float CircleX(int circleObjNum, int circleObjMaX)
    {
        float angle = circleObjNum * 360 / circleObjMaX;
        float x = Mathf.Sin(angle * (Mathf.PI / 180));
        float n = 2.0f; // 倍率
        return x * n;
    }

    float CircleY(int circleObjNum, int circleObjMaX)
    {
        float angle = circleObjNum * 360 / circleObjMaX;
        float y = Mathf.Cos(angle * (Mathf.PI / 180));
        float n = 2.0f; // 倍率
        return y * n;
    }
}
