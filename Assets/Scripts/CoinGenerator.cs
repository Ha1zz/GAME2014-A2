// Source: CoinGenerator
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control when coin will be spawn randomly

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    public GameObject coinPrefab;
    public int numberOfCoin;
    public Transform checkPoint;
    public LayerMask collisionGroundLayer;

    // Start is called before the first frame update
    // Check if the ground is there to spawn the coin
    void Start()
    {
        int i = 0;
        while (i < numberOfCoin)
        {
            float seed = Random.Range(0.0f, 58.0f);
            Vector3 origin = new Vector3(transform.position.x + seed, transform.position.y, transform.position.z);
            Vector3 check = new Vector3(checkPoint.position.x + seed, checkPoint.position.y, checkPoint.position.z);
            var groundHit = Physics2D.Linecast( origin, check, collisionGroundLayer);
            if (groundHit)
            {
                Instantiate(coinPrefab, new Vector3(transform.position.x + seed, transform.position.y, transform.position.z), Quaternion.identity);
                i++;
            }
        }
    }
}
