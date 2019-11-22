using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerafitsize : MonoBehaviour

{
    public GameObject plate;
    
    // Start is called before the first frame update
    void Start()
    {
        // カメラの外枠のスケールをワールド座標系で取得
        float worldScreenHeight = Camera.main.orthographicSize * 2f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        // スプライトのスケールもワールド座標系で取得
        float width = plate.gameObject.GetComponent<Renderer>().bounds.size.x;
        float height = plate.gameObject.GetComponent<Renderer>().bounds.size.y;

        //  両者の比率を出してスプライトのローカル座標系に反映
        transform.localScale = new Vector3(worldScreenWidth / width, worldScreenHeight / height);

        // カメラの中心とスプライトの中心を合わせる
        Vector3 camPos = Camera.main.transform.position;
        camPos.z = 0;
        plate.transform.position = camPos;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
