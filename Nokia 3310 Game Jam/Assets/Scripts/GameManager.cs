using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    float pixelSize = 0.03125f;
    float tileSize = 0.28125f; //Pixelsize times 9, since the middle point off one tile is always 9 pixels away from the next tile
    public float PixelSize { get { return pixelSize; } private set { pixelSize = value; } }
    public float TileSize { get { return tileSize; } private set { tileSize = value; } }

    [SerializeField] float tileSpeed = 3; // How fast tiles will move downwards
    public float TileSpeed { get { return tileSpeed; } private set { tileSpeed = value; } }

    [System.Serializable]
    public class TileRow
    {
        public List<GameObject> tileList = new List<GameObject>();
    }

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
}
