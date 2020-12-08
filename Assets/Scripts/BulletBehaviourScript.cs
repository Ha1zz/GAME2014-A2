// Source: BUlletBehaviourScript
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control bullet

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviourScript : MonoBehaviour
{
    public GameObject player;
    public float verticalSpeed;
    private Vector3 playerPosition;
    private Vector3 firingDirection;
    private Vector3 originPos;
    public float aliveTime;

    // Start is called before the first frame update
    void Start()
    {
        originPos = transform.position;
        player = GameObject.FindGameObjectWithTag("Player");
        playerPosition = player.transform.position;
        firingDirection = Vector3.Normalize(playerPosition - originPos);
    }

    // Update is called once per frame
    void Update()
    {
        Firing(originPos, firingDirection);
        SelfDestruct();
    }

    // Firing the buulet to the target direction
    private void Firing(Vector3 position, Vector3 direction)
    {
        transform.position += direction * verticalSpeed * Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(this.gameObject);
        }
    }
    // Kill it self
    private void SelfDestruct()
    {
        StartCoroutine(Wait(3.0f));
    }

    IEnumerator Wait(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }
}
