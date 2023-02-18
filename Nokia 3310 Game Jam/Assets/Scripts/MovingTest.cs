using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTest : MonoBehaviour
{
    float speed = -3;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0.032f * speed) * Time.deltaTime; 
    }
}
