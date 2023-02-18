using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    float pixelSize = 0.03125f;
    float tileSize = 0.28125f; //Pixelsize times 9;
    public float PixelSize { get { return pixelSize; } private set { pixelSize = value; } }
    public float TileSize { get { return tileSize; } private set { tileSize = value; } }

    [SerializeField] float tileSpeed = 3;
    public float TileSpeed { get { return tileSpeed; } private set { tileSpeed = value; } }

    [SerializeField] List<GameObject> tileList = new List<GameObject>();

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
                break;
            case Direction.Down:
                break;
            case Direction.Left:
                for (int i = 0; i < tileList.Count; i++)
                {
                    if (tileList[i] == currentTile)
                    {
                        if (i != 0)
                            return tileList[i - 1];
                        else
                            return currentTile;
                    }
                }
                break;
            case Direction.Right:
                for (int i = 0; i < tileList.Count; i++)
                {
                    if (tileList[i] == currentTile)
                    {
                        if (i != tileList.Count - 1)
                            return tileList[i + 1];
                        else
                            return currentTile;
                    }
                }

                break;
            default:
                break;
        }
        return currentTile;
    }
}
