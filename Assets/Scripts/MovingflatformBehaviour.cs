// Source: MovingflatformBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control moving platform

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingflatformBehaviour : MonoBehaviour
{
    public Transform upperBoundPoint;
    public Transform lowerBoundPoint;
    private bool isDown = false;
    public float runForce;
    public Rigidbody2D rigidbody2D;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        upperBoundPoint = GameObject.Find("UpperBoundaryPoint").GetComponent<Transform>();
        lowerBoundPoint = GameObject.Find("LowerBoundaryPoint").GetComponent<Transform>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CheckBound();
        Move();
    }

    // Check the position of the platform compare to upper and lower bound
    private void CheckBound()
    {
        if (transform.position.y < lowerBoundPoint.position.y)
        {
            isDown = false;
            rigidbody2D.velocity = Vector3.zero;
        }
        if (transform.position.y > upperBoundPoint.position.y)
        {
            isDown = true;
            rigidbody2D.velocity = Vector3.zero;
        }
    }

    // Move platform
    private void Move()
    {
        if (isDown)
        {
            rigidbody2D.AddForce(Vector2.down * runForce * Time.deltaTime);
        }
        if (!isDown)
        {
            rigidbody2D.AddForce(Vector2.up * runForce * Time.deltaTime);
        }
    }
}
