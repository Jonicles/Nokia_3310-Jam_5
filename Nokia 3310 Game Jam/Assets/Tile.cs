using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    float speed = -3;

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, 0.032f * speed) * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<PlayerMovement>() != null)
        {
            collision.GetComponent<PlayerMovement>().AssignTile(gameObject);
        }
    }
}
