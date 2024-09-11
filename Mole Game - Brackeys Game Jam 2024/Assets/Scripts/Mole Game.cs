using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class MoleGame : MonoBehaviour
{
    public PhysicsMaterial2D rainDropPhysicsMaterial;
    public Camera mainCamera;
    public TextMeshProUGUI savedText;
    public TextMeshProUGUI deadText;

    TileType[,] tileTypes = new TileType[18, 18];
    GameObject[,] tileSprites = new GameObject[18, 18];

    List<GameObject> moles = new List<GameObject>();
    GameObject selectedMole = null;

    float timeSinceRainDrop = 0f;
    float timeUntilRaidDrop = 0.1f;

    ulong numSavedMoles = 0;
    ulong numDeadMoles = 0;

    enum TileType
    {
        Dirt,
        Rock,
        Open,
        Ladder
    }

    void Start()
    {
        GameData gd = GameDataFileHandler.Load();
        numSavedMoles = gd.molesSaved;
        numDeadMoles = gd.molesDead;

        GenerateMapData();
        //GenerateMapVisuals();

        //initialize all tiles in environment
        for (int i = 0; i < tileTypes.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < tileTypes.GetLength(1) - 1; j++)
            {

                int pickTileType = UnityEngine.Random.Range(0, 6);

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

                    GameObject go = new GameObject("Mole");
                    go.AddComponent<HandleRainCollision>();

                    go.transform.localScale = new Vector3(.25f, .25f, 1);
                    go.transform.position = new Vector3(i - 9, j - 9, -1);

                    SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                    Texture2D texture = Resources.Load<Texture2D>("moleStill");

                    Sprite sprite = Sprite.Create
                    (
                        texture,
                        new UnityEngine.Rect(0.0f, 0.0f, texture.width, texture.height),
                        new Vector2(0.5f, 0.5f),
                        (float)texture.width
                    );

                    renderer.sprite = sprite;

                    Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                    go.AddComponent<BoxCollider2D>();

                    moles.Add(go);
                }
            }
        }

        for (int i = 0; i < tileSprites.GetLength(0); i++)
        {
            for (int j = 0; j < tileSprites.GetLength(1); j++)
            {
                GameObject go = new GameObject("Tile Sprite");
                tileSprites[i, j] = go;

                go.transform.position = new Vector3(i - 9, j - 9, 0);

                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                Texture2D texture;

                if (tileTypes[i, j] == TileType.Dirt)
                {
                    texture = Resources.Load<Texture2D>("dirtWall");
                }

                else if (tileTypes[i, j] == TileType.Rock)
                {
                    texture = Resources.Load<Texture2D>("stoneWall");
                }

                else
                {
                    texture = Resources.Load<Texture2D>("blankWall");
                }

                Sprite sprite = Sprite.Create
                (
                    texture,
                    new UnityEngine.Rect(0.0f, 0.0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    (float)texture.width
                );

                renderer.sprite = sprite;
                if (tileTypes[i, j] == TileType.Rock)
                {
                    Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                    body.gravityScale = 0f;
                    body.bodyType = RigidbodyType2D.Static;

                    BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                }
                else if (tileTypes[i, j] == TileType.Dirt)
                {
                    Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                    body.gravityScale = 0f;
                    body.bodyType = RigidbodyType2D.Static;

                    BoxCollider2D collider = go.AddComponent<BoxCollider2D>();
                }
            }
        }
    }

    void GenerateMapData()
    {
        for (int i = 1; i < tileTypes.GetLength(0) - 1; i++)
        {
            tileTypes[i, 0] = TileType.Rock;
            tileTypes[i, 17] = TileType.Open;
        }

        for (int j = 0; j < tileTypes.GetLength(1) - 1; j++)
        {
            tileTypes[0, j] = TileType.Rock;
            tileTypes[17, j] = TileType.Rock;
        }
    }



    void GenerateMapVisuals()
    {

        /*
        for (int row = 0; row < tileSprites.GetLength(0); row++)
        {
            for (int column = 0; column < tileSprites.GetLength(1); column++)
            {
                TileType.TileType tileType = tileTypes[tileTypes[row, column]];
                GameObject obj = (GameObject)Instantiate(tileType.obj, new Vector3((row - 10), (column - 10), 0), Quaternion.identity);
            }
        }
        */
    }

    void Update()
    {
        // Hit TAB to select a random mole!
        if (Input.GetKey("tab"))
        {
            int randomIndex = UnityEngine.Random.Range(0, moles.Count);
            selectedMole = moles[randomIndex];
        }

        if (Input.GetKey("escape"))
        {
            selectedMole = null;
        }

        if (Input.GetKey("n"))
        {
            SceneManager.LoadScene("Mole Game", LoadSceneMode.Single);
        }

        if (selectedMole)
        {
            Rigidbody2D body = selectedMole.GetComponent<Rigidbody2D>();

            if (Input.GetKey("a"))
            {
                body.velocity += new Vector2(-0.1f, 0f);
            }
            if (Input.GetKey("d"))
            {
                body.velocity += new Vector2(0.1f, 0f);
            }
            if (Input.GetKey("w"))
            {
                body.velocity += new Vector2(0f, 0.5f);
            }

            // move camera to selected mole position
            mainCamera.transform.position = new Vector3(selectedMole.transform.position.x, selectedMole.transform.position.y, -10);
        }

        timeSinceRainDrop += Time.deltaTime;

        if (timeSinceRainDrop >= timeUntilRaidDrop)
        {
            timeSinceRainDrop = 0f;

            // Make a single rain drop (a whole bunch of times).
            GameObject go = new GameObject("Rain Drop");
            go.transform.localScale = new Vector2(0.2f, 0.2f);
            go.transform.position = new Vector3(UnityEngine.Random.Range(-8.0f, 6.5f), 16, -1);

            SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
            renderer.color = Color.blue;
            Texture2D texture = Resources.Load<Texture2D>("blankWall");
            Sprite sprite = Sprite.Create
            (
                texture,
                new UnityEngine.Rect(0.0f, 0.0f, texture.width, texture.height),
                new Vector2(0.5f, 0.5f),
                (float)texture.width
            );
            renderer.sprite = sprite;

            go.AddComponent<Rigidbody2D>();
            CircleCollider2D collider = go.AddComponent<CircleCollider2D>();
            collider.sharedMaterial = rainDropPhysicsMaterial;
        }

        List<GameObject> deadMoles = new List<GameObject>();
        foreach (var mole in moles)
        {
            if (mole.tag == "dead")
            {
                deadMoles.Add(mole);
            }
        }

        foreach (var deadMole in deadMoles)
        {
            moles.Remove(deadMole);
            Destroy(deadMole);
        }

        if (deadMoles.Count > 0)
        {
            numDeadMoles += (ulong)deadMoles.Count;
            GameData gd = GameDataFileHandler.Load();
            gd.molesDead = numDeadMoles;
            GameDataFileHandler.Save(gd);
        }

        List<GameObject> successMoles = new List<GameObject>();
        foreach (var mole in moles)
        {
            if (mole.transform.position.y >= 10f)
            {
                successMoles.Add(mole);
            }
        }

        foreach (var successMole in successMoles)
        {
            moles.Remove(successMole);
            Rigidbody2D bdy = successMole.GetComponent<Rigidbody2D>();
            Vector2 velocity = bdy.velocity;
            Vector2 position = successMole.transform.position;
            Destroy(successMole);

            for (int i = 0; i < 64; i++)
            {
                GameObject go = new GameObject("Confetti");

                go.transform.localScale = new Vector3(.05f, .05f, -1);
                go.transform.position = position;

                SpriteRenderer renderer = go.AddComponent<SpriteRenderer>();
                renderer.color = new Color(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
                Texture2D texture = Resources.Load<Texture2D>("blankWall");

                Sprite sprite = Sprite.Create
                (
                    texture,
                    new UnityEngine.Rect(0.0f, 0.0f, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f),
                    (float)texture.width
                );
                renderer.sprite = sprite;

                Rigidbody2D body = go.AddComponent<Rigidbody2D>();
                Vector2 jitter = new Vector2(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f));
                body.velocity = velocity + jitter;
                go.AddComponent<BoxCollider2D>();
            }
        }

        if (successMoles.Count > 0)
        {
            numSavedMoles += (ulong)successMoles.Count;
            GameData gd = GameDataFileHandler.Load();
            gd.molesSaved = numSavedMoles;
            GameDataFileHandler.Save(gd);
        }


        savedText.text = "Saved: " + numSavedMoles;
        deadText.text = "Dead: " + numDeadMoles;
    }
}