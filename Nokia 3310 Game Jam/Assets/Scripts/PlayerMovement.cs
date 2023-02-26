using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float moveTime;
    [SerializeField] public GameObject currentTile;
    [SerializeField] Animator animator;
    public bool alive = true;
    Coroutine moveRoutine;
    private string currentState;
    const string Player_Idle = "player_Idle";
    const string Player_Jump = "player_Jump";

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        if (moveRoutine == null)
        {
            if (currentTile != null)
            {
                transform.position = currentTile.transform.position;
                if (currentTile.GetComponent<Tile>().IsEmpty && alive)
                {
                    Debug.Log("Die because current tile is empty");
                    Die();
                }
            }
        }

        if (alive)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                StartMove(transform.position + new Vector3(0, gameManager.TileSize), true, GameManager.Direction.Up);
                ChangeAnimationState(Player_Jump);
            }
            if (Input.GetKeyDown(KeyCode.A))
            {
                StartMove(transform.position + new Vector3(-gameManager.TileSize, 0), false, GameManager.Direction.Left);
                ChangeAnimationState(Player_Jump);
            }
            if (Input.GetKeyDown(KeyCode.S))
            {
                StartMove(transform.position + new Vector3(0, -gameManager.TileSize), true, GameManager.Direction.Down);
                ChangeAnimationState(Player_Jump);
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                StartMove(transform.position + new Vector3(gameManager.TileSize, 0), false, GameManager.Direction.Right);
                ChangeAnimationState(Player_Jump);
            }
        }
    }

    void StartMove(Vector3 goalPos, bool vertical, GameManager.Direction dir)
    {
        if (moveRoutine == null)
        {
            moveRoutine = StartCoroutine(MovePlayer(goalPos, vertical));
            AssignTile(gameManager.AssignTileToPlayer(currentTile, dir));
        }
    }

    IEnumerator MovePlayer(Vector3 goalPos, bool vertical)
    {
        float elapsedTime = 0;
        float tempValue;
        Vector2 startPos = transform.position;

        while (moveTime > elapsedTime)
        {
            if (vertical)
            {
                tempValue = Mathf.Lerp(startPos.y, goalPos.y, curve.Evaluate(elapsedTime / moveTime));
                transform.position = new Vector3(transform.position.x, tempValue);
            }
            else
            {
                tempValue = Mathf.Lerp(startPos.x, goalPos.x, curve.Evaluate(elapsedTime / moveTime));
                transform.position = new Vector3(tempValue, transform.position.y);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        ChangeAnimationState(Player_Idle);
        //if (vertical)
        //    transform.position = new Vector3(transform.position.x, goalPos.y);
        //else
        //    transform.position = new Vector3(goalPos.x, transform.position.y);

        moveRoutine = null;
        yield return null;
    }

    public void AssignTile(GameObject tile)
    {
        if(currentTile == tile)
        {
            Die();
            return;
        }
        currentTile = tile;
        if (currentTile.GetComponent<Tile>().IsEmpty)
        {
            Debug.Log("Die Because newly assigned tile is empty");
            Die();
        }
        else
        {
            currentTile.GetComponent<Tile>().StartDecay();
        }
    }

    public void Die()
    {
        //transform.position = currentTile.transform.position;
        gameManager.StopSpawner();
        GetComponent<SpriteRenderer>().sortingOrder = -1; //Makes sure player sprite renders under tiles, you have fallen down
        alive = false;
        currentTile = null;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }
}
