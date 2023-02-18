using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] AnimationCurve curve;
    [SerializeField] float moveTime;
    [SerializeField] GameObject currentTile;
    Coroutine moveRoutine;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    // Update is called once per frame
    void Update()
    {
        if (moveRoutine == null)
        {
            transform.position = currentTile.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartMove(transform.position + new Vector3(0, gameManager.TileSize), true, GameManager.Direction.Up);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartMove(transform.position + new Vector3(-gameManager.TileSize, 0), false, GameManager.Direction.Left);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartMove(transform.position + new Vector3(0, -gameManager.TileSize), true, GameManager.Direction.Down);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartMove(transform.position + new Vector3(gameManager.TileSize, 0), false, GameManager.Direction.Right);
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

        //if (vertical)
        //    transform.position = new Vector3(transform.position.x, goalPos.y);
        //else
        //    transform.position = new Vector3(goalPos.x, transform.position.y);

        moveRoutine = null;
        yield return null;
    }

    public void AssignTile(GameObject tile)
    {
        currentTile = tile;
    }
}
