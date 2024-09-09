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
    TileType[,] tileTypes = new TileType[16, 16];
    GameObject[,] tileSprites = new GameObject[16, 16];

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
                
                go.transform.position = new Vector3(i-8, j-8, 0);
                
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

                if (tileTypes[i,j] == TileType.Rock)
                {
                    Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                    body.gravityScale = 0f;
                    body.bodyType = RigidbodyType2D.Static;
                    BoxCollider2D collider = go.AddComponent<BoxCollider2D>();                    
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTileSprites();

        // Make a single rain drop (a whole bunch of times).
        GameObject go = new GameObject("Rain Drop");
        go.transform.localScale = new Vector2(0.1f, 0.1f);
        go.transform.position = new Vector2(UnityEngine.Random.Range(-6, 6), 64);
        
        SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
        renderer.color = Color.blue;                    
        Texture2D texture = Resources.Load<Texture2D>("blank_square");
        Sprite sprite = Sprite.Create
        (
            texture,
            new UnityEngine.Rect(0.0f,0.0f,texture.width,texture.height),
            new Vector2(0.5f, 0.5f),
            (float) texture.width
        );
        renderer.sprite = sprite;

        go.AddComponent<Rigidbody2D>();
        go.AddComponent<BoxCollider2D>();
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
