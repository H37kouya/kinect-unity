using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerControll_2 : MonoBehaviour
{
    public float firstTime;//初期タイム(タイムと同じ。初期タイム取り方わからん...)
    public float nowTime; //タイム
    public Text scoreText; // スコアの UI
    public Text timeText; // タイムの UI

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
    public GameObject Player;

    // player取得
    //GameObject player = GameObject.Find("player");

    public LineRenderer SkeletonLine;

    private GameObject[] bones;
    private LineRenderer[] lines;
    private int[] parIdxs;

    // add new Cube


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
        float firstTime = nowTime;

        // UI を初期化
        score = 0;
        SetCountText();
        timeText.text = "";




        //最初player非表示
        Player.gameObject.SetActive(false);

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
    //updateで1回のみ呼び出すフラグ
    bool CalledOnce = false;
    bool CalledOnce2 = false;

    void Update()
    {

        //ここからkinect関係
        KinectManager manager = KinectManager.Instance;

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
                //表示させないと座標と回転軸を初期化できないと思われる
                bones[i].gameObject.SetActive(true);
                bones[i].transform.localPosition = Vector3.zero;
                bones[i].transform.localRotation = Quaternion.identity;

                //ステージ以外透明
                if (i != 1)
                {
                    bones[i].GetComponent<Renderer>().material.color = Color.clear;
                }

                if (SkeletonLine)
                {
                    lines[i].gameObject.SetActive(false);
                }
            }



        }

        // set the user position in space
        Vector3 posPointMan = manager.GetUserPosition(playerID);
        posPointMan.z = !MirroredMovement ? -posPointMan.z : posPointMan.z;

        // store the initial position(最初initialPosUserIDは0)
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
                    CalledOnce = true;//フラグ

                    //認識して5秒経ったらtrue
                    if (CalledOnce2)
                    {

                        bones[i].gameObject.SetActive(true);
                        Player.gameObject.SetActive(true);


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

                        //ステージ以外透明
                        if (i != 1)
                        {
                            bones[i].GetComponent<Renderer>().material.color = Color.clear;
                        }

                        //unity座標適正化(20倍)
                        bones[i].transform.localPosition = posJoint * 20.0f;
                        bones[i].transform.rotation = rotJoint;

                        //player初期座標設定
                        if (!CalledOnce && bones[1].transform.localPosition != new Vector3(0, 0, 0))
                        {

                            Vector3 playerposfirst = Player.gameObject.transform.position;
                            playerposfirst = new Vector3(bones[1].transform.localPosition.x, bones[1].transform.localPosition.y + 2.0f, bones[1].transform.localPosition.z);
                            Player.gameObject.transform.position = playerposfirst;
                            Debug.Log(playerposfirst);

                        }

                        //player落下検出初期位置戻る
                        Vector3 playerpos = Player.gameObject.transform.position;
                        if (Player.gameObject.transform.position.y < -50)
                        {
                            playerpos = new Vector3(bones[1].transform.localPosition.x, bones[1].transform.localPosition.y + 2.0f, bones[1].transform.localPosition.z);
                            Player.gameObject.transform.position = playerpos;
                        }



                        //時間制限でシーン切り替え
                        if (nowTime < 0)
                        {

                            GameOver("Game Over", "Score" + score.ToString());

                        }

                    }
                }
                else
                {
                    bones[i].gameObject.SetActive(false);
                    //人いなくなったら初期化(びみょい）
                    if (!bones[1].gameObject.active)
                    {
                        CalledOnce = false;
                        CalledOnce2 = false;
                    }

                }


            }
        }

        //タイマー(認識したらスタート)
        if (CalledOnce)
        {
            nowTime -= Time.deltaTime;
        }

        if (CalledOnce2)
        {
            timeText.text = nowTime.ToString("F0");
        }
        else
        {
            float waittime = nowTime - (firstTime - 5);
            timeText.text = waittime.ToString("F0");

            if (waittime < 0)
            {
                nowTime = firstTime;
                CalledOnce2 = true;//フラグ
            }
        }

    }

    // 玉が他のオブジェクトにぶつかった時に呼び出される
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
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

    // UI の表示を更新する(ぶつかったとき）
    void SetCountText()
    {
        int st = score;
        // スコアの表示を更新
        scoreText.text = "Count:" + st.ToString();




        // すべての収集アイテムを獲得した場合
        if (score >= 12)
        {
            nowTime = firstTime - nowTime;
            GameOver(nowTime.ToString() + "秒でクリア!", "Score:12");
        }
    }

    public void GameOver(string resultMessage, string scoreMessage)
    {
        DataSender.resultMessage = resultMessage;  //受け取った引数をstatic変数へ格納
        DataSender.scoreMessage = scoreMessage;
        SceneManager.LoadScene("Result");
    }

}

