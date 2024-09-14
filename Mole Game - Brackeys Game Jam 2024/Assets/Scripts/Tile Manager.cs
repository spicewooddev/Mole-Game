using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JetBrains.Annotations;

public class TileManager : MonoBehaviour
{
    public TileType[,] tileTypes = new TileType[20, 20];
    GameObject[,] tileSprites = new GameObject[20, 20];

    public GameObject dirtPrefab;
    public GameObject stonePrefab;
    public GameObject blankPrefab;

    public bool isTileManagerDone = false;

    public enum TileType
    {
        Dirt,
        Rock,
        Open,
        Ladder
    }

    void Start()
    {
        GenerateMapData();
        GenerateMapVisuals();

        isTileManagerDone = true;
    }

    void Update()
    {

    }

    void GenerateMapData()
    {
        //initialize all tiles in environment
        for (int i = 0; i < tileTypes.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < tileTypes.GetLength(1) - 1; j++)
            {

                int pickTileType = UnityEngine.Random.Range(0, 3);

                if (pickTileType == 0)
                {
                    tileTypes[i, j] = TileType.Dirt;
                }

                else if (pickTileType == 1)
                {
                    tileTypes[i, j] = TileType.Rock;
                }

                else
                {
                    tileTypes[i, j] = TileType.Open;
                }
            }
        }

        for (int i = 0; i < tileTypes.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < tileTypes.GetLength(1) - 1; j++)
            {
                //makes all tiles BUT the edges TileType.Open
                if (tileTypes[i, j] == tileTypes[0, j] ||
                    tileTypes[i, j] == tileTypes[i, 0] ||
                    tileTypes[i, j] == tileTypes[tileTypes.GetLength(0) - 1, j] ||
                    tileTypes[i, j] == tileTypes[i, tileTypes.GetLength(1) - 1])
                {
                    tileTypes[i, j] = TileType.Open;
                }
            }
        }
    }

    void GenerateMapVisuals()
    {
        for (int i = 0; i < tileSprites.GetLength(0); i++)
        {
            for (int j = 0; j < tileSprites.GetLength(1); j++)
            {
                GameObject obj;

                if (tileTypes[i, j] == TileType.Dirt)
                {
                    obj = dirtPrefab;
                }

                else if (tileTypes[i, j] == TileType.Rock)
                {
                    obj = stonePrefab;
                }

                else
                {
                    obj = blankPrefab;
                }

                obj = (GameObject)Instantiate(obj, new Vector3((i - 10), (j - 10), 0), Quaternion.identity);
            }
        }
    }
}