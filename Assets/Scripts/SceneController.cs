// Source: SceneController
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Print point for game over scene

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SceneController : MonoBehaviour
{
    public TMP_Text pointText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        pointText.text = PlayerPrefs.GetInt("point").ToString();
    }
}
