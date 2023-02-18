using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float moveTime;
    [SerializeField] GameObject currentTile;
    float pixelSize = 0.032f;
    float tileSize;
    Coroutine moveRoutine;
    private void Start()
    {
        tileSize = pixelSize * 8;
    }
    // Update is called once per frame
    void Update()
    {
        if(moveRoutine == null)
        {
            transform.position = currentTile.transform.position;
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            StartMove(transform.position + new Vector3(0, tileSize), true);
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            StartMove(transform.position + new Vector3(-tileSize, 0), false);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            StartMove(transform.position + new Vector3(0, -tileSize), true);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            StartMove(transform.position + new Vector3(tileSize, 0), false);
        }
    }

    void StartMove(Vector3 goalPos, bool vertical)
    {
        if (moveRoutine == null)
        {
            moveRoutine = StartCoroutine(MovePlayer(goalPos, vertical));
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
