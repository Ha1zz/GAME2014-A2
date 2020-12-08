// Source: ChangeSceneScript
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Change Scene when pressed

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ChangeSceneScript : MonoBehaviour
{
    private string sceneName;
    private AudioClip buttonClip;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        buttonClip = Resources.Load<AudioClip>("SFX/PressButton");
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // change Scene and play sound when pressed
    public void changeScene(string sceneName)
    {
        audioSource.PlayOneShot(buttonClip);
        SceneManager.LoadScene(sceneName);
    }
}
