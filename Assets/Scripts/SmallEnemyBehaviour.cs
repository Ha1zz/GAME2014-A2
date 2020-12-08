// Source: SmallEnemyBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control small enemy

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SmallEnemyBehaviour : MonoBehaviour
{
    public GameObject coinPrefab;
    public float runForce;
    public Rigidbody2D rigidbody2D;
    public bool isGroundAhead;
    public bool onRamp;
    public RampDirection rampDirection;
    public Transform lookInFrontPoint;
    public Transform lookAheadPoint;
    public LayerMask collisionGroundLayer;
    public LayerMask collisionWallLayer;
    public Transform lookAbovePoint;
    public LayerMask collisionPlayerLayer;
    public Transform bulletSpawnPoint;

    private Animator animator;

    [Header("Music System")]
    public GameObject musicManager;
    public AudioSource audioSource;
    public AudioClip clip;
    public bool canPlay = true;

    [Header("Bullet Firing")]
    public LOS enemyLOS;
    public GameObject bulletPrefab;
    public float fireDelay;
    public PlayerBehaviour player;


    // Start is called before the first frame update
    void Start()
    {
        musicManager = GameObject.Find("MusicManager");
        clip = Resources.Load<AudioClip>("SFX/EnemyDeath");
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        rampDirection = RampDirection.NONE;
        player = GameObject.FindObjectOfType<PlayerBehaviour>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (hasLOS())
        {
            FireBullet();
        }
        LookAbove();
        LookInFront();
        LookAhead();
        Move();
    }

    private void FireBullet()
    {
        //delay bullet firing
        if (Time.frameCount % fireDelay == 0)
        {
            var playerPosition = player.transform.position;
            var firingDirection = Vector3.Normalize(playerPosition - bulletSpawnPoint.position);
            Instantiate(bulletPrefab, new Vector3(bulletSpawnPoint.position.x, bulletSpawnPoint.position.y, bulletSpawnPoint.position.z), Quaternion.identity);
            StartCoroutine(Wait(2.0f));
        }
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
    }

    // Check if has Line Of Sine with Player
    private bool hasLOS()
    {
        if (enemyLOS.colliders.Count > 0 && enemyLOS.colliders != null)
        {
            if (enemyLOS.collidesWith.gameObject.name == "Player" && enemyLOS.colliders[0].gameObject.name == "Player")
            {
                return true;
            }
        }
        return false;
    }

    // Play sound
    void PlaySound(AudioClip aClip)
    {
        musicManager.GetComponent<MusicController>().PlayAudio(aClip);
    }

    // Move smaller enemy
    private void Move()
    {
        
        if (isGroundAhead)
        {
            //transform.Rotate(Vector3.forward * 8.0f);
            rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime * transform.localScale.x);
            //rigidbody2D.AddForce(Vector2.left * runForce * Time.deltaTime);
            if (onRamp)
            {
                if (rampDirection == RampDirection.UP)
                {
                    rigidbody2D.AddForce(Vector2.up * runForce * 0.5f * Time.deltaTime);
                }
                else
                {
                    rigidbody2D.AddForce(Vector2.down * runForce * 0.25f * Time.deltaTime);
                }

                StartCoroutine(Rotate());
            }
            else
            {
                StartCoroutine(Normalize());
            }


            rigidbody2D.velocity *= 0.90f;
        }
        else if (onRamp)
        {
            StartCoroutine(Rotate());
        }
        else
        {
            FlipX();
        }

    }

    // Check if ramp infront 
    private void LookInFront()
    {
        var wallHit = Physics2D.Linecast(transform.position, lookInFrontPoint.position, collisionWallLayer);
        if (wallHit)
        {
            if (!wallHit.collider.CompareTag("Ramps"))
            {
                //if (!onRamp && transform.rotation.z == 0.0f)
                if (!onRamp && transform.rotation.z == 0.0f)
                {
                    FlipX();
                }
                rampDirection = RampDirection.DOWN;
            }
            else
            {
                rampDirection = RampDirection.UP;
            }
        }

        Debug.DrawLine(transform.position, lookInFrontPoint.position, Color.red);
    }

    // Check if look ahead
    private void LookAhead()
    {
        var groundHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionGroundLayer);
        if (groundHit)
        {
            if (groundHit.collider.CompareTag("Ramps"))
            {
                onRamp = true;
            }

            if (groundHit.collider.CompareTag("Grounds"))
            {
                onRamp = false;
            }

            isGroundAhead = true;
        }
        else
        {
            isGroundAhead = false;
        }

        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.yellow);
    }

    // check if player jump on smaller enemy and self-destruct
    private void LookAbove()
    {
        var playerHit = Physics2D.Linecast(transform.position, lookAbovePoint.position, collisionPlayerLayer);
        if (playerHit)
        {
            if (playerHit.collider.CompareTag("Player"))
            {
                PlaySound(clip);
                Instantiate(coinPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity);
                Destroy(this.gameObject);
            }
        }
        Debug.DrawLine(transform.position, lookAbovePoint.position, Color.green);
    }

    IEnumerator Rotate()
    {
        yield return new WaitForSeconds(0.05f);
        if ((transform.localScale.x == 1.0f) && (rampDirection == RampDirection.UP))
        {
            // left and up
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);
        }
        else if ((transform.localScale.x == 1.0f) && (rampDirection == RampDirection.DOWN))
        {
            // left and down
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 26.0f);
        }
        else if ((transform.localScale.x == -1.0f) && (rampDirection == RampDirection.UP))
        {
            // right and up
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, 26.0f);
        }
        else if ((transform.localScale.x == -1.0f) && (rampDirection == RampDirection.DOWN))
        {
            // right and down
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, -26.0f);
        }
    }

    IEnumerator Normalize()
    {
        yield return new WaitForSeconds(0.05f);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void FlipX()
    {
        transform.localScale = new Vector3(transform.localScale.x * -1.0f, transform.localScale.y, transform.localScale.z);
        transform.Rotate(Vector3.forward * -8.0f);
    }

}
