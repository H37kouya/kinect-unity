using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControll_2 : MonoBehaviour
{

    //public float speed; // 動く速さ
    public Text scoreText; // スコアの UI
    public Text winText; // リザルトの UI

    //private Rigidbody rb; // Rididbody
    private int score; // スコア
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
    private Quaternion rotation;

    public PlayerControll_2(Quaternion rotation)
    {
        this.rotation = rotation;
    }

    void Start()
    {
        // Rigidbody を取得
        //rb = GetComponent<Rigidbody>();

        // UI を初期化
        score = 0;
        SetCountText();
        winText.text = "";

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
       

        // array holding the skeleton lines
        lines = new LineRenderer[bones.Length];

        // Timer = new TimerBusiness();
        // Timer.SetStopTime(30, true);

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
    }

    void Update()
    {
        // カーソルキーの入力を取得
        var moveHorizontal = Input.GetAxis("Horizontal");
        var moveVertical = Input.GetAxis("Vertical");

        //// カーソルキーの入力に合わせて移動方向を設定
       
        //if (Input.GetKey(KeyCode.UpArrow))
        //{
        //    transform.Rotate(50 * Time.deltaTime, 0, 0);
        //}
        //if (Input.GetKey(KeyCode.DownArrow))
        //{
        //    transform.Rotate(-50 * Time.deltaTime, 0, 0);
        //}
        //if (Input.GetKey(KeyCode.RightArrow))
        //{
        //    transform.Rotate(0, 0, -50 * Time.deltaTime);
        //}
        //if (Input.GetKey(KeyCode.LeftArrow))
        //{
        //    transform.Rotate(0, 0, 50 * Time.deltaTime);
        //}


        //ここからkinect関係
        KinectManager manager = KinectManager.Instance;

        Debug.Log(manager.IsInitialized());

        // get 1st player
        uint playerID = manager != null ? manager.GetPlayer1ID() : 0;

        // タイマーの更新
        // Timer.TimerUpdate();

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

                    bones[i].transform.localPosition = posJoint * 20.0f;
                    bones[i].transform.rotation = rotJoint;
                }
                else
                {
                    bones[i].gameObject.SetActive(false);
                }

            }
        }
    }

    // 玉が他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider other)
    {
        // ぶつかったオブジェクトが収集アイテムだった場合
        if (other.gameObject.CompareTag("Pick up"))
        {
            // その収集アイテムを非表示にします
            other.gameObject.SetActive(false);

            // スコアを加算します
            score = score + 1;

            // UI の表示を更新します
            SetCountText();
        }
    }

    // UI の表示を更新する
    void SetCountText()
    {
        // スコアの表示を更新
        scoreText.text = "Count: " + score.ToString();

        // すべての収集アイテムを獲得した場合
        if (score >= 12)
        {
            // リザルトの表示を更新
            winText.text = "You Win!";
        }
    }
}
