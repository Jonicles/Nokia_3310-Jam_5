using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] bool isEmpty = false;
    public bool IsEmpty { get { return isEmpty; } private set { isEmpty = value; } }

    private void Start()
    {
        if (isEmpty)
        {
            EmptyTile();
        }
        gameManager = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0, gameManager.PixelSize * -gameManager.TileSpeed) * Time.deltaTime;
    }

    public void StartDecay()
    {

    }

    public void EmptyTile()
    {
        isEmpty = true;
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
