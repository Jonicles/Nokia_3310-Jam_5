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


    void Start()
    {
        gameManager = GameManager.Instance;
        //currentTimeValue = timeBetweenSpawns;
    }

    private void Update()
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

    public void StartSpawner()
    {

    }

    public void StopSpawner()
    {

    }

    public void CreateNewRow()
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

        gameManager.AddNewTileRow(newRow);
    }

}
