using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Rotator : MonoBehaviour
{
    //XYどちら使うか
    public bool UseX = true;
    public bool UseY = false;
    // 回転速度
    public float rotatespeed;
    // カメラのオブジェクト
    public GameObject CameraAxisObject;
    // 腰のオブジェクト
    public GameObject BaseRotateParameter;
    // 手のオブジェクト
    public GameObject RotateParameter;
    //最大回転速度
    public float MoveRotateMultiplier;

    void Start()
    {
        if (UseX == UseY)
        {
            UseX = true;
            UseY = false;
        }

        StartCoroutine("OnRotator");
    }

    IEnumerator OnRotator()
    {
        // カメラのオブジェクト
        Vector3 Axis;
        // 左手のオブジェクト
        Vector3 RotateParameterPos;
        // 腰のオブジェクト
        Vector3 BaseRotateParameterPos;
        Vector3 Torque = Vector3.zero;
        Vector3 TorqueSpeed = Vector3.zero;

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
                Vector3 ForcePos = ReverseVecByBasePos(BaseRotateParameterPos, RotateParameterPos);

                Vector3 TorqueNow = GetTorqueNow(ForcePos);
                TorqueSpeed = Torque - TorqueNow;
                Torque = TorqueNow;
                rotaterforce.AddTorque(Torque); //MoveRotateMultiplier * (TorqueSpeed - rotaterforce.velocity));
            }
            else
            {
                yield return new WaitForSeconds(0.5f);
            }

            yield return null;
        }
    }

    // base よりobjebecが低かったらtrue
    bool LowerBaseVec(Vector3 Base, Vector3 objVec)
    {
        if (UseX)
        {
            return LowerBaseVecX(Base, objVec);
        }

        if (UseY)
        {
            return LowerBaseVecY(Base, objVec);
        }

        return LowerBaseVecX(Base, objVec);
    }


    // base よりobjebecが低かったらtrue
    bool LowerBaseVecY(Vector3 Base, Vector3 objVec)
    {
        return Base.y < objVec.y;
    }

    // base よりobjebecが低かったらtrue
    bool LowerBaseVecX(Vector3 Base, Vector3 objVec)
    {
        return Base.x < objVec.x;
    }

    // 条件付きでvectorを反転
    Vector3 ReverseVecByBasePos(Vector3 Base, Vector3 objVec)
    {
        if (LowerBaseVec(Base, objVec))
        {
            return objVec * -1;
        }

        return objVec;
    }

    Vector3 GetTorqueNow(Vector3 ForcePos)
    {
        if (UseX)
        {
            return Vector3.forward * Mathf.PI * ForcePos.y * rotatespeed / 30;
        }

        if (UseY)
        {
            return Vector3.forward * Mathf.PI * ForcePos.y * rotatespeed;
        }

        return Vector3.forward * Mathf.PI * ForcePos.x * rotatespeed;
    }
}
