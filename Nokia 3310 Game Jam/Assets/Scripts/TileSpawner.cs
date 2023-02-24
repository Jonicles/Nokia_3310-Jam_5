using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileSpawner : MonoBehaviour
{
    [SerializeField] GameObject tilePrefab;
    Vector3 spawnPos = new Vector3(-0.5f, 1.5f);
    int tileOnRowAmount = 5;
    float timeBetweenSpawns = 1f;
    float currentTimeValue;
    GameManager gameManager;
    bool isActive = true;
    [SerializeField] PlayerMovement player;

    void Start()
    {
        gameManager = GameManager.Instance;
        //currentTimeValue = timeBetweenSpawns;
    }

    private void Update()
    {
        if (player.alive && isActive)
        {
            if (currentTimeValue > 0)
            {
                currentTimeValue -= Time.deltaTime * gameManager.TileSpeed;
            }
            else
            {
                CreateNewRow();
                currentTimeValue = timeBetweenSpawns;
            }
        }
    }

    public void StartSpawner()
    {
        isActive = true;
    }

    public void StopSpawner()
    {
        isActive = false;
    }


    public void CreateNewRow()
    {
        bool playerPath = IsPlayerOnPath();

        if (playerPath)
        {
            TileRow tempRow = new TileRow();
            bool path = false;

            while (!path && player.alive)
            {
                tempRow = eh();
                path = gameManager.CheckPath(tempRow);
                
                if (!path)
                {
                    for (int i = 5 - 1; i >= 0; i--)
                    {
                        Destroy(tempRow.tileList[i]);
                        //newRow.tileList.RemoveAt(i);
                    }
                }
            }
            gameManager.AddNewTileRow(tempRow);
        }
        else
            StopSpawner();
    }

    private bool IsPlayerOnPath()
    {
        TileRow newRow = new TileRow();
        for (int i = 0; i < tileOnRowAmount; i++)
        {
            newRow.tileList.Add(Instantiate(tilePrefab, spawnPos + new Vector3(gameManager.TileSize * i, 0), Quaternion.identity));
        }
        bool path = gameManager.CheckPath(newRow);

        for (int i = 5 - 1; i >= 0; i--)
        {
            Destroy(newRow.tileList[i]);
            //newRow.tileList.RemoveAt(i);
        }

        return path;
    }

    public TileRow eh()
    {
        TileRow newRow = new TileRow();
        for (int i = 0; i < tileOnRowAmount; i++)
        {
            newRow.tileList.Add(Instantiate(tilePrefab, spawnPos + new Vector3(gameManager.TileSize * i, 0), Quaternion.identity));
            int tempNumber = Random.Range(0, 3);
            if (tempNumber == 0)
            {
                newRow.tileList[i].gameObject.GetComponent<Tile>().EmptyTile();
            }
        }

        return newRow;
    }

}
