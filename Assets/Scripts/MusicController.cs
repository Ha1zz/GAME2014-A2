// Source: MusicController
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control music

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = UnityEngine.Debug;

public class MusicController : MonoBehaviour
{
    public AudioClip menuClip;
    public AudioClip playClip;
    public AudioClip overClip;
    public AudioClip introClip;
    public AudioSource audioSource;
    private string sceneName;
    private bool canPlay;

    // Load music clip from file and play music depend on the screen
    // Start is called before the first frame update
    void Start()
    {
        menuClip = Resources.Load<AudioClip>("Sounds/MainMenu");
        playClip = Resources.Load<AudioClip>("Sounds/GamePlay");
        overClip = Resources.Load<AudioClip>("Sounds/GameOver");
        introClip = Resources.Load<AudioClip>("Sounds/Instruction");
        sceneName = SceneManager.GetActiveScene().name;
        if (sceneName == "GameOverScreen")
        {
            audioSource.PlayOneShot(overClip);
        }
        if (sceneName == "GamePlayScreen")
        {
            audioSource.PlayOneShot(playClip);
        }
        if (sceneName == "InstructionScreen")
        {
            audioSource.PlayOneShot(introClip);
        }
        if (sceneName == "MainMenuScreen")
        {
            audioSource.PlayOneShot(menuClip);
        }
    }

    // Play audio once
    public void PlayAudio(AudioClip clip)
    {

        if (canPlay)
            canPlay = false;
        GetComponent<AudioSource>().PlayOneShot(clip);

        StartCoroutine(Reset());
    }

    IEnumerator Reset()
    {
        yield return new WaitForSeconds(.2f);
        canPlay = true;
    }
}
