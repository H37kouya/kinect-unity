using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PatternEffectController : MonoBehaviour
{
    public Vector3[] outputPositions;

    public GameObject Cube0;
    public GameObject Sphere;
    public GameObject Cube1;
    public GameObject Cube2;
    public GameObject Cube3;
    public GameObject Cube4;
    public GameObject Cube5;
    public GameObject Cube6;
    public GameObject Cube7;
    public GameObject Cube8;
    public GameObject Cube9;
    public GameObject Sphere10;
    public GameObject Capsule11;
    public GameObject Cylinder12;
    public GameObject Sphere13;
    public GameObject Capsule14;
    public GameObject Cylinder15;
    public GameObject Tree16;
    public GameObject Cube17;
    public GameObject Cube18;
    public GameObject Cube19;


    public GameObject[] PatternObject;

    public float[] VisibleTime;
    public float VisibleTimeMax = 5.0f;
    public bool[] VisibleTimeBool;

    public float FadeSpeed = 0.01f;
    public Color FadeSpeedColor = new Color(0.01f, 0.01f, 0.01f, 0.01f);

    // 待機画面表示中かどうか
    public bool WaitingDisplay = true;

    // 待機画面のゲームオブジェクト
    public GameObject WaitingDisplayObject;

    void Start()
    {
        PatternObject = new GameObject[] {
            Cube0, Sphere, Cube1, Cube2, // 0 - 3
            Cube3, Cube4, Cube5, Cube6, // 4 - 7
            Cube7, Cube8, Cube9, Sphere10, // 8 - 11
            Cylinder12, Sphere13, Capsule14, Cylinder15,// 12 - 15
            Tree16, Cube17, Cube18, Cube19,// 16 - 19
        };

        // 配列の初期化 領域の確保
        VisibleTime = new float[PatternObject.Length];
        VisibleTimeBool = new bool[PatternObject.Length];
        outputPositions = new Vector3[PatternObject.Length];

        // 配列を既定の値で初期化
        for (int i = 0; i < PatternObject.Length; i++)
        {
            PatternObjectDisVisible(i);
            VisibleTimeBool[i] = false;
        }

        // 待機画面の表示 (初期化)
        WaitingDisplayOn();
    }

    void Update()
    {
        // get the joint position
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            if (manager.IsUserDetected())
            {
                // 待機画面表示中だったら非表示にする
                WaitingDisplayDown();

                // kinect 関連の処理
                MainKinectControll(manager);
            }
        }

        bool countCheck = false;

        for (int joint = 0; joint < PatternObject.Length; joint++)
        {
            Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;

            if (VisibleTimeBool[joint])
            {
                VisibleTime[joint] += Time.deltaTime;
                countCheck = true;
                FadeIn(joint, PatternObjectColor); // フェードイン を行う
            }

            if (VisibleTime[joint] > VisibleTimeMax)
            {
                countCheck = true;
                FadeOut(joint, PatternObjectColor); // フェードアウト を行う
            }
        }

        // 待機画面表示してなくて、タイマーが動かなかったとき。
        if (!countCheck && !WaitingDisplay)
        {
            // 待機画面表示処理
            WaitingDisplayOn();
        }
    }

    // 座標が特定の誤差以下かを判別する
    bool EqualCoordinate(float coord1, float coord2)
    {
        return Mathf.Abs(coord1 - coord2) < 0.1;
    }

    // 座標がほぼ同じか判別する
    bool EqualVector3(Vector3 vec1, Vector3 vec2)
    {
        if (!EqualCoordinate(vec1.x, vec2.x))
        {
            return false;
        }

        if (!EqualCoordinate(vec1.y, vec2.y))
        {
            return false;
        }

        if (!EqualCoordinate(vec1.z, vec2.z))
        {
            return false;
        }

        return true;
    }

    // kinect 関連の処理
    void MainKinectControll(KinectManager manager)
    {
        uint userId = manager.GetPlayer1ID();

        for (int joint = 0; joint < PatternObject.Length; joint++)
        {
            if (manager.IsJointTracked(userId, joint) && !VisibleTimeBool[joint])
            {
                // output the joint position for easy tracking
                Vector3 jointPos = manager.GetJointPosition(userId, joint);

                if (!EqualVector3(jointPos, PatternObject[joint].transform.position))
                {
                    jointPos.x = 20 * Mathf.Pow(jointPos.x, 3); // 通常使用で発散しないギリギリライン
                    PatternObjectTimeStart(joint); // timer のカウント開始
                    PatternObject[joint].transform.position = jointPos;

                    // Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;
                    // PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                    //     PatternObjectColor.r, PatternObjectColor.g, PatternObjectColor.b, 0
                    // );
                    PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                        0, 0, 0, 0
                    );
                }
                // outputPositions[joint] = jointPos;
            }
        }
    }

    // PatternObjectの表示
    void PatternObjectVisible(int joint)
    {
        PatternObject[joint].SetActive(true);
    }

    // PatternObjectの非表示
    void PatternObjectDisVisible(int joint)
    {
        PatternObject[joint].SetActive(false);
    }

    // timer と pattern object を初期化
    void PatternObjectTimeInitialize(int joint)
    {
        VisibleTime[joint] = 0.0f;
        VisibleTimeBool[joint] = false;
        PatternObjectDisVisible(joint);
    }

    void PatternObjectTimeStart(int joint)
    {
        VisibleTime[joint] = 0.0f;
        VisibleTimeBool[joint] = true;
        PatternObjectVisible(joint);
    }

    // 待機画面表示に変更
    void WaitingDisplayOn()
    {
        WaitingDisplay = true;
        WaitingDisplayObject.SetActive(true);
    }

    // 待機画面非表示へ変更
    void WaitingDisplayDown()
    {
        if (WaitingDisplay)
        {
            WaitingDisplay = false;
            WaitingDisplayObject.SetActive(false);
        }
    }

    // fade in の実装
    void FadeIn(int joint, Color PatternObjectColor)
    {
        // Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;
        if (PatternObject[joint].GetComponent<Renderer>().material.color.a < 1.0f)
        {
            if (PatternObjectColor.a + FadeSpeedColor.a < 1.0f)
            {
                PatternObject[joint].GetComponent<Renderer>().material.color = PatternObjectColor + FadeSpeedColor;
            }
            else
            {
                // PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                //     PatternObjectColor.r, PatternObjectColor.g, PatternObjectColor.b, 1f
                // );
                PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                    1f, 1f, 1f, 1f
                );
            }
        }
    }

    // fade out の実装
    void FadeOut(int joint, Color PatternObjectColor)
    {
        if (PatternObjectColor.a - FadeSpeedColor.a > 0)
        {
            PatternObject[joint].GetComponent<Renderer>().material.color = PatternObjectColor - FadeSpeedColor;
        }
        else
        {
            PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                PatternObjectColor.r, PatternObjectColor.g, PatternObjectColor.b, 0f
            );
        }

        // アルファの値が 0 のとき
        if (PatternObject[joint].GetComponent<Renderer>().material.color.a == 0f)
        {
            // timer と patternobjectを初期化
            PatternObjectTimeInitialize(joint);
        }
    }

}
