// Source: CoinBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control player


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalBehaviour : MonoBehaviour
{
    // Change to game over when collider with player
    private void OnTriggerEnter2D(Collider2D other)
    {
        // respawn
        if (other.gameObject.CompareTag("Player"))
        {
            SceneManager.LoadScene("GameOverScreen");
        }
    }
}
