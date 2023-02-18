using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, gameManager.PixelSize * -gameManager.TileSpeed) * Time.deltaTime;
    }
}
