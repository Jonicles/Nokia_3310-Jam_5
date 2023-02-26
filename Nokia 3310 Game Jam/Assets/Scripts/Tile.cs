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
    string currentState;
    const string Tile_Decay = "tile_Decay";
    const string Tile_Empty = "tile_Empty";
    [SerializeField] Animator animator;

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
        ChangeAnimationState(Tile_Decay);
        if (pointsCollected == false)
        {
            pointManager.AddPoints(Random.Range(10, 15));
            pointsCollected = true;
        }
        

    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }

    public void EmptyTile()
    {
        isEmpty = true;
        ChangeAnimationState(Tile_Empty);
        //gameObject.GetComponent<SpriteRenderer>().sprite = null;
    }
}
