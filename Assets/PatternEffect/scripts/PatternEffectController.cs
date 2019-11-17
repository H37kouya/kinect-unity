using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine.UI;
public class PatternEffectController : MonoBehaviour
{
    // the joint we want to track
    // public KinectWrapper.NuiSkeletonPositionIndex joint = KinectWrapper.NuiSkeletonPositionIndex.HandRight;

    // public KinectWrapper.NuiSkeletonPositionIndex[] joints;

    // joint position at the moment, in Kinect coordinates
    // public Vector3 outputPosition;

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
    public bool[] VisibleTimeBool;

    public float FadeSpeed = 0.01f;
    public Color FadeSpeedColor = new Color(0.01f, 0.01f, 0.01f, 0.01f);
    // if it is saving data to a csv file or not
    // public bool isSaving = false;

    // how many seconds to save data into the csv file, or 0 to save non-stop
    // public float secondsToSave = 0f;

    // path to the csv file (;-limited)
    // public string saveFilePath = "joint_pos.csv";


    // start time of data saving to csv file
    // private float saveStartTime = -1f;

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

    }

    void Update()
    {
        // get the joint position
        KinectManager manager = KinectManager.Instance;

        if (manager && manager.IsInitialized())
        {
            if (manager.IsUserDetected())
            {
                uint userId = manager.GetPlayer1ID();

                for (int joint = 0; joint < PatternObject.Length; joint++)
                {
                    if (manager.IsJointTracked(userId, joint) && !VisibleTimeBool[joint])
                    {
                        // output the joint position for easy tracking
                        Vector3 jointPos = manager.GetJointPosition(userId, joint);

                        // Debug.Log(jointPos);

                        if (!EqualVector3(jointPos, PatternObject[joint].transform.position))
                        {
                            jointPos.x = 20 * Mathf.Pow(jointPos.x, 3); // 通常使用で発散しないギリギリライン
                            VisibleTime[joint] = 0.0f;
                            VisibleTimeBool[joint] = true;
                            PatternObjectVisible(joint);
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
        }

        for (int joint = 0; joint < PatternObject.Length; joint++)
        {
            Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;

            if (VisibleTimeBool[joint])
            {
                VisibleTime[joint] += Time.deltaTime;

                if (PatternObject[joint].GetComponent<Renderer>().material.color.a < 1.0f)
                {
                    FadeIn(joint);
                }
            }

            if (VisibleTime[joint] > 5.0f)
            {
                // FadeOut(joint);
                if ((PatternObjectColor - FadeSpeedColor).a > 0)
                {
                    PatternObject[joint].GetComponent<Renderer>().material.color = PatternObjectColor - FadeSpeedColor;
                }
                else
                {
                    PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
                        PatternObjectColor.r, PatternObjectColor.g, PatternObjectColor.b, 0f
                    );
                }

                if (PatternObject[joint].GetComponent<Renderer>().material.color.a == 0f)
                {
                    VisibleTime[joint] = 0.0f;
                    VisibleTimeBool[joint] = false;
                    PatternObjectDisVisible(joint);
                }
            }
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

    // fade outの実装
    // void FadeOut(int joint)
    // {
    //     Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;

    //     Debug.Log((PatternObjectColor - FadeSpeedColor).a);

    //     if ((PatternObjectColor - FadeSpeedColor).a > 0)
    //     {
    //         PatternObject[joint].GetComponent<Renderer>().material.color = PatternObjectColor - FadeSpeedColor;
    //     }
    //     else
    //     {
    //         PatternObject[joint].GetComponent<Renderer>().material.color = new Color(
    //             PatternObjectColor.r, PatternObjectColor.g, PatternObjectColor.b, 0f
    //         );
    //     }
    // }

    // fade in の実装
    void FadeIn(int joint)
    {
        Color PatternObjectColor = PatternObject[joint].GetComponent<Renderer>().material.color;

        if ((PatternObjectColor + FadeSpeedColor).a < 1.0f)
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
