using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RotaterPanel : MonoBehaviour
{
    public GameObject PlatePanel;

    // controllObjを更新する間隔
    public float TimeDiff = 0.5f;
    void Start()
    {
        StartCoroutine("controllObj");
    }

    IEnumerator controllObj()
    {
        while (true)
        {
            if (DataCenter.IsAllDetected() && DataCenter.GameMode == 1)
            {
                PlatePanel.SetActive(false);
                yield return new WaitForSeconds(10);
            }

            if (DataCenter.IsAllDetected() && DataCenter.GameMode == 2)
            {
                PlatePanel.SetActive(true);
                yield return new WaitForSeconds(10);
            }

            yield return new WaitForSeconds(TimeDiff);
        }
    }
}
