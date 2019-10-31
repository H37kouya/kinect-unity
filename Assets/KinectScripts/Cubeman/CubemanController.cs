using UnityEngine;
using System;
using System.Collections;

// MonoBehaviourはUnityEngineの継承
public class CubemanController : MonoBehaviour
{
    public bool MoveVertically = false;
    public bool MirroredMovement = false;

    //public GameObject debugText;

    public GameObject Hip_Center;
    public GameObject Spine;
    public GameObject Shoulder_Center;
    public GameObject Head;
    public GameObject Shoulder_Left;
    public GameObject Elbow_Left;
    public GameObject Wrist_Left;
    public GameObject Hand_Left;
    public GameObject Shoulder_Right;
    public GameObject Elbow_Right;
    public GameObject Wrist_Right;
    public GameObject Hand_Right;
    public GameObject Hip_Left;
    public GameObject Knee_Left;
    public GameObject Ankle_Left;
    public GameObject Foot_Left;
    public GameObject Hip_Right;
    public GameObject Knee_Right;
    public GameObject Ankle_Right;
    public GameObject Foot_Right;

    // add new Cube
    public GameObject Cube;

    public LineRenderer SkeletonLine;

    private GameObject[] bones;
    private LineRenderer[] lines;
    private int[] parIdxs;

    // add new Cube
    private GameObject[] cubes;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private Vector3 initialPosOffset = Vector3.zero;
    private uint initialPosUserID = 0;

    // メインカメラ
    private Camera _mainCamera;

    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log("Debug Start" + i);
        }
        //store bones in a list for easier access
        bones = new GameObject[] {
            Hip_Center, Spine, Shoulder_Center, Head,  // 0 - 3
			Shoulder_Left, Elbow_Left, Wrist_Left, Hand_Left,  // 4 - 7
			Shoulder_Right, Elbow_Right, Wrist_Right, Hand_Right,  // 8 - 11
			Hip_Left, Knee_Left, Ankle_Left, Foot_Left,  // 12 - 15
			Hip_Right, Knee_Right, Ankle_Right, Foot_Right  // 16 - 19
		};

        parIdxs = new int[] {
            0, 0, 1, 2,
            2, 4, 5, 6,
            2, 8, 9, 10,
            0, 12, 13, 14,
            0, 16, 17, 18
        };

        // new setup cubes
        cubes = new GameObject[] {
            Cube
        };

        // array holding the skeleton lines
        lines = new LineRenderer[bones.Length];

        if (SkeletonLine)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = Instantiate(SkeletonLine) as LineRenderer;
                lines[i].transform.parent = transform;
            }
        }

        initialPosition = transform.position;
        initialRotation = transform.rotation;
        //transform.rotation = Quaternion.identity;

        // カメラオブジェクトを取得します
        GameObject mainCamera = GameObject.Find("Main Camera");
        _mainCamera = mainCamera.GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        KinectManager manager = KinectManager.Instance;

        // get 1st player
        uint playerID = manager != null ? manager.GetPlayer1ID() : 0;

        if (playerID <= 0)
        {
            // reset the pointman position and rotation
            if (transform.position != initialPosition)
            {
                transform.position = initialPosition;
            }

            if (transform.rotation != initialRotation)
            {
                transform.rotation = initialRotation;
            }

            for (int i = 0; i < bones.Length; i++)
            {
                bones[i].gameObject.SetActive(true);

                bones[i].transform.localPosition = Vector3.zero;
                bones[i].transform.localRotation = Quaternion.identity;

                if (SkeletonLine)
                {
                    lines[i].gameObject.SetActive(false);
                }
            }

            cubes[0].gameObject.SetActive(true);
            cubes[0].transform.localPosition = Vector3.zero;
            cubes[0].transform.localRotation = Quaternion.identity;
            cubes[0].GetComponent<Renderer>().material.color = Color.clear;


            return;
        }

        // set the user position in space
        Vector3 posPointMan = manager.GetUserPosition(playerID);
        posPointMan.z = !MirroredMovement ? -posPointMan.z : posPointMan.z;

        // store the initial position
        if (initialPosUserID != playerID)
        {
            initialPosUserID = playerID;
            initialPosOffset = transform.position - (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));
        }

        transform.position = initialPosOffset + (MoveVertically ? posPointMan : new Vector3(posPointMan.x, 0, posPointMan.z));

        // update the local positions of the bones
        for (int i = 0; i < bones.Length; i++)
        {
            if (bones[i] != null)
            {
                int joint = MirroredMovement ? KinectWrapper.GetSkeletonMirroredJoint(i) : i;

                if (manager.IsJointTracked(playerID, joint))
                {
                    bones[i].gameObject.SetActive(true);
                    cubes[0].gameObject.SetActive(true);
                    cubes[0].transform.localPosition = Vector3.zero;
                    cubes[0].transform.localRotation = Quaternion.identity;
                    cubes[0].GetComponent<Renderer>().material.color = Color.clear;


                    Vector3 posJoint = manager.GetJointPosition(playerID, joint);
                    posJoint.z = !MirroredMovement ? -posJoint.z : posJoint.z;

                    Quaternion rotJoint = manager.GetJointOrientation(playerID, joint, !MirroredMovement);
                    rotJoint = initialRotation * rotJoint;

                    posJoint -= posPointMan;

                    if (MirroredMovement)
                    {
                        posJoint.x = -posJoint.x;
                        posJoint.z = -posJoint.z;
                    }

                    bones[i].transform.localPosition = posJoint;
                    bones[i].transform.rotation = rotJoint;
                }
                else
                {
                    bones[i].gameObject.SetActive(false);
                }
            }
        }

        if (SkeletonLine)
        {
            // オブジェクトの個数回回している
            for (int i = 0; i < bones.Length; i++)
            {
                bool bLineDrawn = false;

                if (bones[i] != null)
                {
                    if (bones[i].gameObject.activeSelf)
                    {
                        Vector3 posJoint = bones[i].transform.position;

                        int parI = parIdxs[i];
                        Vector3 posParent = bones[parI].transform.position;

                        Vector3 posLocalJoint = bones[i].transform.localPosition;
                        Vector3 posLocalParent = bones[parI].transform.localPosition;

                        if (bones[parI].gameObject.activeSelf)
                        {
                            lines[i].gameObject.SetActive(true);

                            if (i == 7)
                            {
                                float ty = (posLocalJoint.y <= 0) ? posLocalJoint.y : posLocalJoint.y * 4.0f;
                                cubes[0].transform.position = new Vector3(
                                    posLocalJoint.x * 7,
                                    ty,
                                    posLocalJoint.z
                                );
                            }

                            if (i == 0)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.red;
                            }
                            else if (i == 1 || i == 8 || i == 16)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.black;
                            }
                            else if (i == 2 || i == 9 || i == 17)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.blue;
                            }
                            else if (i == 3 || i == 10 || i == 19)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.green;
                            }
                            else if (i == 4 || i == 11 || i == 18)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.cyan;
                            }
                            else if (i == 5 || i == 12 || i == 15)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.yellow;
                            }
                            else if (i == 6 || i == 13)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.magenta;
                            }
                            else if (i == 7 || i == 14)
                            {
                                cubes[0].GetComponent<Renderer>().material.color = Color.clear;
                            }

                            // Debug.Log(posParent.ToString()); // 座標 (x, y, z)
                            // Debug.Log(bones[i].ToString()); // バグる
                            // Debug.Log(posParent.GetType()); // オブジェクトの型の名前
                            // Debug.Log(bones[i].name); // オブジェクトの名前 ファイル名?
                            // Debug.Log(bones[i].GetType()); // return GameObject only
                            // Debug.Log(i); // wait for moveのときactiveSelf内に入ってこない

                            //lines[i].SetVertexCount(2);

                            lines[i].SetPosition(0, posParent);
                            lines[i].SetPosition(1, posJoint);

                            bLineDrawn = true;


                        }
                    }
                }

                if (!bLineDrawn)
                {
                    lines[i].gameObject.SetActive(false);
                }
            }
        }

    }

    private Vector3 getScreenTopLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = _mainCamera.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        return topLeft;
    }

    private Vector3 getScreenBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        return bottomRight;
    }

    private Vector3 getScreenWidth()
    {
        // 画面の右下を取得
        Vector3 topRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0.0f, 0.0f));
        // 上下反転させる
        topRight.Scale(new Vector3(1f, -1f, 1f));
        return topRight;
    }

    private Vector3 getScreenHeight()
    {
        // 画面の右下を取得
        Vector3 bottomLeft = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomLeft.Scale(new Vector3(1f, -1f, 1f));
        return bottomLeft;
    }

}
