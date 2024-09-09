using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TileType
{
    Dirt,
    Rock,
    Open,
    Ladder
}

public class MoleGame : MonoBehaviour
{

    TileType[,] tileTypes = new TileType[64, 64];
    GameObject[,] tileSprites = new GameObject[64, 64];

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < tileTypes.GetLength(0); i++)
        {
            for (int j = 0; j < tileTypes.GetLength(1); j++)
            {
                int pickTileType = UnityEngine.Random.Range(0, 3);
                if (pickTileType == 0) {
                    tileTypes[i,j] = TileType.Dirt;
                } else if (pickTileType == 1) {
                    tileTypes[i,j] = TileType.Rock;
                } else {
                    tileTypes[i,j] = TileType.Open;
                }
            }
        }

        for (int i = 0; i < tileSprites.GetLength(0); i++)
        {
            for (int j = 0; j < tileSprites.GetLength(1); j++)
            {
                GameObject go = new GameObject("Tile Sprite");
                tileSprites[i,j] = go;
                
                go.transform.localScale = new Vector2(0.5f, 0.5f);
                go.transform.position = new Vector3((i-32) / 2.0f, (j-32) / 2.0f, 0);
                
                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                Texture2D texture = Resources.Load<Texture2D>("blank_square");
                Sprite sprite = Sprite.Create
                (
                    texture,
                    new UnityEngine.Rect(0.0f,0.0f,texture.width,texture.height),
                    new Vector2(0.5f, 0.5f),
                    (float) texture.width
                );
                renderer.sprite = sprite;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTileSprites();
    }

    void UpdateTileSprites()
    {           
        for (int i = 0; i < tileTypes.GetLength(0); i++)
        {
            for (int j = 0; j < tileTypes.GetLength(1); j++)
            {
                GameObject go = tileSprites[i,j];
                SpriteRenderer renderer = go.GetComponent<SpriteRenderer>();
                if (tileTypes[i,j] == TileType.Dirt)
                {
                    renderer.color = new Color(139f / 256f, 69f / 256f, 19f / 256f, 1.0f);
                }
                else if (tileTypes[i,j] == TileType.Rock)
                {
                    renderer.color = Color.gray;
                }
                else if (tileTypes[i,j] == TileType.Open)
                {
                    renderer.color = Color.black;
                }
                else
                {
                    renderer.color = Color.red;                    
                }
            }
        }
    }
}
