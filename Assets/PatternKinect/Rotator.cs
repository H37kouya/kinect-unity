using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour
{
    // 回転速度
    public float rotatespeed;
    // カメラのオブジェクト
    public GameObject CameraAxisObject;
    // 腰のオブジェクト数
    public GameObject BaseRotateParameter;
    // 手のオブジェクト
    public GameObject RotateParameter;

    void Start()
    {
        StartCoroutine("OnRotator");
    }

    IEnumerator OnRotator()
    {
        // カメラのオブジェクト
        Vector3 Axis;
        // 左手のオブジェクト
        Vector3 RotateParameterPos;
        //腰のオブジェクト
        Vector3 BaseRotateParameterPos;

        while (true)
        {
            if (DataCenter.IsAllDetected() && DataCenter.GameMode == 2)
            {
                // カメラのオブジェクトのポジション
                Axis = CameraAxisObject.transform.position;
                // 腰のオブジェクトのポジション
                BaseRotateParameterPos = BaseRotateParameter.transform.position;
                // 左手のオブジェクトのポジション
                RotateParameterPos = RotateParameter.transform.position;
                // Rigidbodyの付加
                Rigidbody rotaterforce = this.GetComponent<Rigidbody>();
                // 条件付きでvectorを判定する
                ReverseVecByBasePos(BaseRotateParameterPos, RotateParameterPos);

                rotaterforce.AddTorque(Vector3.forward * Mathf.PI * RotateParameterPos.y * rotatespeed);
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }

    // base よりobjebecが低かったらtrue
    bool LowerBaseVecY(Vector3 Base, Vector3 objVec)
    {
        return Base.y < objVec.y;
    }

    // 条件付きでvectorを反転
    Vector3 ReverseVecByBasePos(Vector3 Base, Vector3 objVec)
    {
        if (LowerBaseVecY(Base, objVec))
        {
            return objVec * -1;
        }

        return objVec;
    }
}
