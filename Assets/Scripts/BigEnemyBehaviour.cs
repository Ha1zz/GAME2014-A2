// Source: BigEnemyBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control Big Enemy behaviour


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigEnemyBehaviour : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform upperBoundPoint;
    public Rigidbody2D rigidbody2D;
    public Transform lookBelowPoint;
    public LayerMask collisionGroundLayer;
    public Transform lookAbovePoint;
    public LayerMask collisionPlayerLayer;
    public bool isGroundAhead;
    public float runForce;

    public GameObject musicManager;
    public AudioSource audioSource;
    public AudioClip clip;
    public bool canPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        upperBoundPoint = GameObject.Find("UpperBoundaryPoint").GetComponent<Transform>();
        musicManager = GameObject.Find("MusicManager");
    }

    void PlaySound(AudioClip aClip)
    {
        musicManager.GetComponent<MusicController>().PlayAudio(aClip);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        LookAbove();
        CheckLowerBound();
        Move();
    }

    // Check if enemy touch the ground then change to the opposite direction
    private void CheckLowerBound()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookBelowPoint.position, collisionGroundLayer);
        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Grounds"))
            {
                isGroundAhead = true;
                rigidbody2D.velocity = Vector3.zero;
            }
        }
        else
        {
            if (transform.position.y >= upperBoundPoint.position.y)
            {
                rigidbody2D.velocity = Vector3.zero;
                isGroundAhead = false;
            }
        }
        Debug.DrawLine(transform.position, lookBelowPoint.position, Color.yellow);
    }

    // Move big enemy
    private void Move()
    {
        if (isGroundAhead == false)
        {
            rigidbody2D.AddForce(Vector2.down * runForce * Time.deltaTime);
        }
        if (isGroundAhead == true)
        {
            rigidbody2D.AddForce(Vector2.up * runForce * Time.deltaTime);
        }
    }

    // Check if player jump on of the big enemy to kill them
    private void LookAbove()
    {
        var playerHit = Physics2D.Linecast(transform.position, lookAbovePoint.position, collisionPlayerLayer);
        if (playerHit)
        {
            Debug.Log("CHECK");
            if (playerHit.collider.CompareTag("Player"))
            {
                PlaySound(clip);
                Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Destroy(gameObject);
            }
        }
        Debug.DrawLine(transform.position, lookAbovePoint.position, Color.green);
    }
}
