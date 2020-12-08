// Source: CoinBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control coin

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinBehaviour : MonoBehaviour
{

    public GameObject musicManager;
    public AudioSource audioSource;
    public AudioClip clip;
    public bool canPlay = true;

    void Start()
    {
        musicManager = GameObject.Find("MusicManager");
    }
    // Play sound
    void PlaySound(AudioClip aClip)
    {
        musicManager.GetComponent<MusicController>().PlayAudio(aClip);
    }

    // Destroy coin if collect with Player
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") )
        {
            PlaySound(clip);
            Destroy(gameObject);
        }
        if (other.gameObject.CompareTag("Deathplanes"))
        {
            Destroy(gameObject);
        }
    }
}
