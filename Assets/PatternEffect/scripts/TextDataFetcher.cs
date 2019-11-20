using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextDataFetcher : MonoBehaviour
{

    public Text resultMessageText;
    public Text scoreMessageText;

    // Use this for initialization
    void Start()
    {
        resultMessageText.text = DataSender.resultMessage;
        scoreMessageText.text = DataSender.scoreMessage;
    }

    // Update is called once per frame
    void Update()
    {

    }
}