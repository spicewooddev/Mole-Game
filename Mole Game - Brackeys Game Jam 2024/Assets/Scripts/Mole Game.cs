using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
enum TileType
{
    Dirt,
    Rock,
    Open,
    Ladder
}
*/

public class MoleGame : MonoBehaviour
{
    public TileType[] tileTypes;
    public GameObject[,] tileSprites = new GameObject[24, 24];
    public int[,] tiles = new int[24, 24];

    void Start()
    {
        GenerateMapData();
        GenerateMapVisuals();
    }

    void GenerateMapData()
    {
        //initialize all tiles in environment
        for (int row = 0; row < tiles.GetLength(0); row++)
        {
            for (int column = 0; column < tiles.GetLength(1); column++)
            {
                int pickTileType = UnityEngine.Random.Range(0, 3);

                if (pickTileType == 1)
                {
                    //dirt wall tiles
                    tiles[row, column] = 1;
                }

                else if (pickTileType == 2)
                {
                    //stone wall tiles
                    tiles[row, column] = 2;
                }

                else
                {
                    //empty tiles
                    tiles[row, column] = 0;
                }
            }
        }
    }


    void GenerateMapVisuals()
    {
        for (int row = 0; row < tiles.GetLength(0); row++)
        {
            for (int column = 0; column < tiles.GetLength(1); column++)
            {
                TileType tileType = tileTypes[tiles[row, column]];
                GameObject go = (GameObject)Instantiate(tileType.go, new Vector3((row - 12) * 0.45f, (column - 12) * 0.45f, 0), Quaternion.identity);
                go.transform.localScale = new Vector2(2f, 2f);
            }
        }
    }

    void Update()
    {

    }
}