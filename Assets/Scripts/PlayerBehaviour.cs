// Source: PlayerBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control player


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum RampDirection
{
    NONE,
    UP,
    DOWN
}

[System.Serializable]
public enum PlayerAnimationType
{
    IDLE,
    RUN,
    JUMP,
}

public class PlayerBehaviour : MonoBehaviour
{
    [Header("Controls")]
    public Joystick joystick;
    public float joystickHorizontalSensitivity;
    public float joystickVerticalSensitivity;
    public float horizontalForce;
    public float verticalForce;



    [Header("Platform Detection")]
    public Transform camera;
    public Rigidbody2D rigidbody2D;
    public bool isJumping;
    public bool onRamp;
    public bool isGrounded;
    public Transform lookBelowPoint;
    public Transform lookAheadPoint;
    public LayerMask collisionGroundLayer;
    public LayerMask collisionRampLayer;
    public Transform spawnPoint;
    public RampDirection rampDirection;
    public float rampForceFactor;

    private Animator animator;
    private RaycastHit2D groundHit;
    private RaycastHit2D wallHit;

    [Header("PlayerStats")]
    public int life;
    public int point;

    public TMP_Text lifeText;
    public TMP_Text pointText;

    [Header("Soundsystem")]
    public GameObject musicManager;
    public AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip deathClip;
    public AudioClip jumpClip;
    public bool canPlay = true;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        musicManager = GameObject.Find("MusicManager");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //isOnSlope();
        CheckAhead();
        UpdateLifeAndPoint();
        CheckAlive();
        CheckBelow();
        Move();
    }

    // Play sound
    void PlaySound(AudioClip aClip)
    {
        musicManager.GetComponent<MusicController>().PlayAudio(aClip);
    }

    // Check if player is alive
    private void CheckAlive()
    {
        if (life <= 0)
        {
            PlayerPrefs.SetInt("life", life);
            PlayerPrefs.SetInt("point", point);
            PlaySound(deathClip);
            //Destroy(this.gameObject);
            SceneManager.LoadScene("GameOverScreen");
        }
    }

    // Check with other object tag
    //private void OnTriggerEnter2D(Collider2D other)
    //{
    //    if (other.gameObject.CompareTag("Deathplanes"))
    //    {
    //        PlaySound(hitClip);
    //        life--;
    //        transform.position = spawnPoint.position;
    //    }
    //    if (other.gameObject.CompareTag("Coin"))
    //    {
    //        point++;
    //    }
    //    if (other.gameObject.CompareTag("Enemy"))
    //    {
    //        PlaySound(hitClip);
    //        life--;
    //        transform.position = spawnPoint.position;
    //    }
    //}

    //Compare tag
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Deathplanes"))
        {
            PlaySound(hitClip);
            life--;
            transform.position = spawnPoint.position;
        }
        if (other.gameObject.CompareTag("Coin"))
        {
            point++;
        }
        if (other.gameObject.CompareTag("Enemy"))
        {
            PlaySound(hitClip);
            life--;
            transform.position = spawnPoint.position;
        }
        if (other.gameObject.CompareTag("Bullet"))
        {
            PlaySound(hitClip);
            rigidbody2D.velocity = Vector2.zero;
        }
    }

    // Move player with joystick
    private void Move()
    {
        if (!isJumping)
        {
            if (joystick.Horizontal > joystickHorizontalSensitivity)
            {
                rigidbody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
                transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
                if (onRamp && rampDirection == RampDirection.UP)
                {
                    rigidbody2D.AddForce(Vector2.up * verticalForce * rampForceFactor);
                }
                else if (onRamp && rampDirection == RampDirection.DOWN)
                {
                    rigidbody2D.AddForce(Vector2.down * verticalForce * rampForceFactor);
                }
                animator.SetInteger("PlayerState", (int)PlayerAnimationType.RUN);
            }
            else if (joystick.Horizontal < -joystickHorizontalSensitivity)
            {
                rigidbody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
                transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
                if (onRamp && rampDirection == RampDirection.UP)
                {
                    rigidbody2D.AddForce(Vector2.up * verticalForce * rampForceFactor);
                }
                else if (onRamp && rampDirection == RampDirection.DOWN)
                {
                    rigidbody2D.AddForce(Vector2.down * verticalForce * rampForceFactor);
                }
                animator.SetInteger("PlayerState", (int)PlayerAnimationType.RUN);
            }
            else
            {
                animator.SetInteger("PlayerState", (int)PlayerAnimationType.IDLE);
                rigidbody2D.velocity = Vector2.zero;
            }
        }


        if ((joystick.Vertical > joystickVerticalSensitivity) && (!isJumping))
        {
            rigidbody2D.AddForce(Vector2.up * verticalForce);
            animator.SetInteger("PlayerState", (int)PlayerAnimationType.JUMP);
            isJumping = true;
            PlaySound(jumpClip);
            if (joystick.Horizontal > joystickHorizontalSensitivity)
            {
                rigidbody2D.AddForce(Vector2.right * horizontalForce * Time.deltaTime);
            }
            else
            {
                rigidbody2D.AddForce(Vector2.left * horizontalForce * Time.deltaTime);
            }
        }
        else
        {
            isJumping = false;
        }
    }


    // Check ahead for Ramp direction
    private void CheckAhead()
    {
        if (!isGrounded)
        {
            rampDirection = RampDirection.NONE;
            return;
        }

        wallHit = Physics2D.Linecast(transform.position, lookAheadPoint.position, collisionRampLayer);
        if (wallHit)
        {
            if (wallHit.collider.CompareTag("Ramps"))
            {
                rampDirection = RampDirection.UP;
                onRamp = true;
            }
        }
        else
        {
            if (isOnSlope()) 
            {
                rampDirection = RampDirection.DOWN;
            }
        }
        Debug.DrawLine(transform.position, lookAheadPoint.position, Color.yellow);
    }

    private bool isOnSlope()
    {
        if (!isGrounded)
        {
            onRamp = false;
            return false;
        }
        // (groundHit.collider.CompareTag("Grounds") && wallHit.collider.CompareTag("Ramps"))

        if (groundHit.collider.CompareTag("Ramps"))
        {
            onRamp = true;
            return true;
        }

        onRamp = false;
        return false;
    }


    // Check if player is jumping
    private void CheckBelow()
    {
        groundHit = Physics2D.Linecast(transform.position, lookBelowPoint.position, collisionGroundLayer);
        if (groundHit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            isJumping = true;
        }
        Debug.DrawLine(transform.position, lookBelowPoint.position, Color.white);
    }

    // Update Life and Point text
    private void UpdateLifeAndPoint()
    {
        if (life > 0)
        {
            lifeText.text = life.ToString();
            pointText.text = point.ToString();
        }
    }
}
