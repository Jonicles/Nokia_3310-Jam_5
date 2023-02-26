using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    PointManager pointManager;    
    GameManager gameManager;
    [SerializeField] bool isEmpty = false;
    //[SerializeField] float decayTime = 2;
    float currentTime;
    bool isDecaying = false;
    bool pointsCollected = false;

    public bool IsEmpty { get { return isEmpty; } private set { isEmpty = value; } }

    private void Start()
    {
        gameManager = GameManager.Instance;
        pointManager = GameObject.Find("PointManager").GetComponent<PointManager>();

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

        if (pointsCollected == false)
        {
            pointManager.AddPoints(Random.Range(10, 15));
            pointsCollected = true;
        }
        

    }

    public void EmptyTile()
    {
        isEmpty = true;
        GetComponent<SpriteRenderer>().sprite = null;
        
    }
}
