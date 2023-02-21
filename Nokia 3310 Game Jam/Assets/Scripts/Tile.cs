using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] bool isEmpty = false;
    //[SerializeField] float decayTime = 2;
    float currentTime;
    bool isDecaying = false;
    public bool IsEmpty { get { return isEmpty; } private set { isEmpty = value; } }

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, gameManager.TileSize) * Time.deltaTime * gameManager.TileSpeed;

        if (isDecaying)
        {
            if (currentTime < gameManager.TileDecayTime)
            {
                currentTime += Time.deltaTime;
            }
            else
            {
                EmptyTile();
                isDecaying = false;
            }
        }
    }

    public void StartDecay()
    {
        isDecaying = true;
    }

    public void EmptyTile()
    {
        isEmpty = true;
        GetComponent<SpriteRenderer>().sprite = null;
    }
}
