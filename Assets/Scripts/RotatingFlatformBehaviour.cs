// Source: RotatingFlatformBehaviour
// Author: Tran Thien Phu 
// StudentNumber: 101160213
// Date last Modifield: 08/12/2020
// Description: Control rotating platform

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class RotatingFlatformBehaviour : MonoBehaviour
{
    public float rotateZ;
    private int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        transform.Rotate(0.0f, 0.0f, rotateZ, Space.World);
        StartCoroutine(Rotate());
    }

    // Update is called once per frame
    void Update()
    {
        if (count == 240)
        {
            transform.Rotate(0.0f, 0.0f, 90.0f, Space.Self);
        }

        if (count <= 480)
        {
            count++;
        }
        if (count >= 480)
        {
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.Self);
            count = 0;
        }
    }

    IEnumerator Rotate()
    {
        transform.Rotate(0.0f, 0.0f, 90.0f, Space.World);
        yield return new WaitForSeconds(5);
        transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
        //Return();
    }

    IEnumerator Return()
    {
        {
            transform.Rotate(0.0f, 0.0f, 0.0f, Space.World);
            yield return new WaitForSeconds(1);
        }
    }
}
