using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TileRow
{
    public List<GameObject> tileList = new List<GameObject>();
}
public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    float pixelSize = 0.03125f;
    float tileSize = 0.25f; //Pixelsize times 8, since the middle point off one tile is always 8 pixels away from the next tile
    public float PixelSize { get { return pixelSize; } private set { pixelSize = value; } }
    public float TileSize { get { return tileSize; } private set { tileSize = value; } }

    [SerializeField] float tileSpeed = 2; // How fast tiles will move downwards, this should change during the game which means tile spawn rate also need to change
    public float TileSpeed { get { return tileSpeed; } private set { tileSpeed = value; } }

    [SerializeField] float tileDecayTime = 2; // How fast tiles will decay and become empty
    public float TileDecayTime { get { return tileDecayTime; } private set { tileDecayTime = value; } }

    [SerializeField] GameObject startingArea;

    [SerializeField] TileSpawner spawner;



    [SerializeField] List<TileRow> tileRowList = new List<TileRow>();

    PlayerMovement player;

    public enum Direction { Up, Down, Left, Right };
    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<PlayerMovement>();
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (tileRowList.Count == 13)
        {
            RemoveBottomTileRow();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    public bool CheckPath(TileRow potentialRow)
    {
        List<GameObject> tilesToCheck = new List<GameObject>();

        for (int i = 0; i < potentialRow.tileList.Count; i++)
        {
            if (!tileRowList[tileRowList.Count - 1].tileList[i].GetComponent<Tile>().IsEmpty && !potentialRow.tileList[i].GetComponent<Tile>().IsEmpty)
            {
                tilesToCheck.Add(tileRowList[tileRowList.Count - 1].tileList[i]);
            }
        }
        List<GameObject> knownTiles = new List<GameObject>();
        knownTiles = Recursion(tilesToCheck, knownTiles);

        if (knownTiles.Contains(player.currentTile))
        {
            return true;
        }
        else
            return false;
        //If there are no adjacent tiles the path will be false
    }

    private List<GameObject> Recursion(List<GameObject> tilesToCheck, List<GameObject> knownTiles)
    {
        List<GameObject> connectedTiles = new List<GameObject>();

        foreach (GameObject tileGO in tilesToCheck)
        {
            if (!knownTiles.Contains(tileGO))
            {
                knownTiles.Add(tileGO);

                for (int i = 0; i < tileRowList.Count; i++)
                {
                    for (int j = 0; j < tileRowList[i].tileList.Count; j++)
                    {
                        if (tileRowList[i].tileList[j] == tileGO)
                        {
                            //Found coordinate of tileGO

                            //Check Left tile
                            if (j != 0)
                            {
                                if (!tileRowList[i].tileList[j - 1].GetComponent<Tile>().IsEmpty)
                                {
                                    if (!knownTiles.Contains(tileRowList[i].tileList[j - 1]))
                                    {
                                        connectedTiles.Add(tileRowList[i].tileList[j - 1]);
                                    }
                                }
                            }

                            //Check Right tile
                            if (j != 4)
                            {
                                if (!tileRowList[i].tileList[j + 1].GetComponent<Tile>().IsEmpty)
                                {
                                    if (!knownTiles.Contains(tileRowList[i].tileList[j + 1]))
                                    {
                                        connectedTiles.Add(tileRowList[i].tileList[j + 1]);
                                    }
                                }
                            }

                            //Check Bottom Tile
                            if (i != 0)
                            {
                                if (!tileRowList[i - 1].tileList[j].GetComponent<Tile>().IsEmpty)
                                {
                                    if (!knownTiles.Contains(tileRowList[i - 1].tileList[j]))
                                    {
                                        connectedTiles.Add(tileRowList[i - 1].tileList[j]);
                                    }
                                }
                            }

                            //Check top tile
                            if (i != tileRowList.Count - 1)
                            {
                                if (!tileRowList[i + 1].tileList[j].GetComponent<Tile>().IsEmpty)
                                {
                                    if (!knownTiles.Contains(tileRowList[i + 1].tileList[j]))
                                    {
                                        connectedTiles.Add(tileRowList[i + 1].tileList[j]);
                                    }
                                }
                            }
                        }
                    }
                }

                Recursion(connectedTiles, knownTiles);
            }
        }

        return knownTiles;
    }

    public void AddNewTileRow(TileRow newRow)
    {
        tileRowList.Add(newRow);
    }

    public void RemoveBottomTileRow()
    {
        for (int i = 5 - 1; i >= 0; i--)
        {
            Destroy(tileRowList[0].tileList[i].gameObject);
            tileRowList[0].tileList.RemoveAt(i);
        }
        tileRowList.Remove(tileRowList[0]);
    }

    public GameObject AssignTileToPlayer(GameObject currentTile, Direction dir)
    {
        switch (dir)
        {
            case Direction.Up:
                for (int i = 0; i < tileRowList.Count; i++)
                {
                    for (int j = 0; j < tileRowList[i].tileList.Count; j++)
                    {
                        if (tileRowList[i].tileList[j] == currentTile)
                        {
                            //Found players current tile position
                            if (i != tileRowList.Count - 1)
                                return tileRowList[i + 1].tileList[j]; //Returns tile that is over player
                            else
                                return currentTile;
                        }
                    }
                }
                break;
            case Direction.Down:
                for (int i = 0; i < tileRowList.Count; i++)
                {
                    for (int j = 0; j < tileRowList[i].tileList.Count; j++)
                    {
                        if (tileRowList[i].tileList[j] == currentTile)
                        {
                            //Found players current tile position
                            if (i != 0)
                                return tileRowList[i - 1].tileList[j]; //Returns tile that is under player
                            else
                                return currentTile;
                        }
                    }
                }
                break;
            case Direction.Left:
                for (int i = 0; i < tileRowList.Count; i++)
                {
                    for (int j = 0; j < tileRowList[i].tileList.Count; j++)
                    {
                        if (tileRowList[i].tileList[j] == currentTile)
                        {
                            //Found players current tile position
                            if (j != 0)
                                return tileRowList[i].tileList[j - 1]; //Returns tile to the left
                            else
                                return currentTile;
                        }
                    }
                }
                break;
            case Direction.Right:
                for (int i = 0; i < tileRowList.Count; i++)
                {
                    for (int j = 0; j < tileRowList[i].tileList.Count; j++)
                    {
                        if (tileRowList[i].tileList[j] == currentTile)
                        {
                            //Found players current tile position
                            if (j != tileRowList[i].tileList.Count - 1)
                                return tileRowList[i].tileList[j + 1]; //Returns tile to the right
                            else
                                return currentTile;
                        }
                    }
                }

                break;
            default:
                break;
        }
        return currentTile;
    }

    public void Restart()
    {
        GameObject startArea = Instantiate(startingArea, new Vector3(0, 0.5f, 0), Quaternion.identity);
        for (int i = 0; i < tileRowList.Count; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                Destroy(tileRowList[i].tileList[j]);
            }
        }

        tileRowList.Clear();
        InsertStartArea(startArea);
        StartSpawner();
    }

    private void InsertStartArea(GameObject startarea)
    {
        for (int i = 0; i < startarea.transform.childCount; i++)
        {
            TileRow newTileRow = new TileRow();

            for (int j = 0; j < startarea.transform.GetChild(i).childCount; j++)
            {
                newTileRow.tileList.Add(startarea.transform.GetChild(i).transform.GetChild(j).gameObject);
            }

            tileRowList.Add(newTileRow);
        }

        player.alive = true;
        player.currentTile = tileRowList[0].tileList[2];
        player.gameObject.transform.position = tileRowList[0].tileList[2].transform.position;
        player.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
    }

    public void StartSpawner()
    {
        spawner.StartSpawner();
    }

    public void StopSpawner()
    {
        spawner.StopSpawner();
    }
}
