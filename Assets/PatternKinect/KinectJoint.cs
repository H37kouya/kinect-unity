using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class KinectJoint : MonoBehaviour
{
    public GameObject KinectJointObject;

    // kinect上の関節の番号
    public int JointKinectNumber;
    // 関節の番号 (自分で振る)
    public int JointNumber;
    // 座標のタイミングの更新間隔
    public float TimeDiff = 0.5f;

    // 感知したかどうか
    public bool IsDetected = false;

    // 座標の補正 x
    public int correctionX = 1;
    // 座標の補正 y
    public int correctionY = 1;

    void Start()
    {
        StartCoroutine("GetJoint");
    }

    // joint の処理
    IEnumerator GetJoint()
    {
        while (true)
        {
            KinectManager manager = KinectManager.Instance;

            if (manager && manager.IsInitialized() && manager.IsUserDetected())
            {
                // userIdを取得
                uint userId = manager.GetPlayer1ID();

                // 感知したかどうか
                SetIsDetected(true);

                // jointが動いたかどうか
                if (manager.IsJointTracked(userId, JointKinectNumber))
                {
                    JointPosObjUpdate(manager, userId); // 関節のpositionを更新
                }
            }
            else
            {
                // 感知したかどうか
                SetIsDetected(false);
            }

            yield return new WaitForSeconds(TimeDiff);
        }
    }

    // 関節の position を更新
    void JointPosObjUpdate(KinectManager manager, uint userId)
    {
        // output the joint position for easy tracking
        Vector3 jointPos = manager.GetJointPosition(userId, JointKinectNumber);
        // 座標の補正をする
        Vector3 correctionPos = JointPosObjByFirstFunction(jointPos);
        // 関節のpositionを更新
        KinectJointObject.transform.localPosition = correctionPos;
    }

    Vector3 JointPosObjByFirstFunction(Vector3 jointPos)
    {
        return new Vector3(jointPos.x * correctionX, jointPos.y * correctionY, jointPos.z);
    }

    Vector3 JointPosObjByThreeFunction(Vector3 jointPos)
    {
        return new Vector3(
            Mathf.Pow(jointPos.x, 3) * correctionX,
            Mathf.Pow(jointPos.y, 3) * correctionY,
            jointPos.z
        );
    }

    // 人を感知したかどうかをセットする
    void SetIsDetected(bool _IsDetected)
    {
        IsDetected = _IsDetected; // このオブジェクト内の IsDetected を更新
        DataCenter.IsDetected[JointNumber] = _IsDetected; // DataCenter の IsDetected を更新
    }
}
