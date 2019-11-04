using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

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

    public GameObject[] PatternObject;

    public float[] VisibleTime;
    public bool[] VisibleTimeBool;
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
            Cube0, Sphere, Cube1, Cube2,
            Cube3, Cube4, Cube5, Cube6,
            Cube7, Cube8
        };

        VisibleTime = new float[PatternObject.Length];
        VisibleTimeBool = new bool[PatternObject.Length];
        outputPositions = new Vector3[PatternObject.Length];

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
                        }
                        // outputPositions[joint] = jointPos;
                    }
                }
            }
        }

        for (int joint = 0; joint < PatternObject.Length; joint++)
        {
            if (VisibleTimeBool[joint])
            {
                VisibleTime[joint] += Time.deltaTime;
            }

            if (VisibleTime[joint] > 5.0f)
            {
                VisibleTime[joint] = 0.0f;
                VisibleTimeBool[joint] = false;
                PatternObjectDisVisible(joint);
            }
        }
    }

    bool EqualCoordinate(float coord1, float coord2)
    {
        return Mathf.Abs(coord1 - coord2) < 0.1;
    }

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

    void PatternObjectVisible(int joint)
    {
        PatternObject[joint].SetActive(true);
    }

    void PatternObjectDisVisible(int joint)
    {
        PatternObject[joint].SetActive(false);
    }


}
