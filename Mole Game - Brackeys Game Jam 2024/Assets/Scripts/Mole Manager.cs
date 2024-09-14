using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Build.Content;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MoleManager : MonoBehaviour
{
    public Camera mainCamera;

    private TileManager tileManagerScript;

    public TextMeshProUGUI savedText;
    public TextMeshProUGUI deadText;

    List<GameObject> moles = new List<GameObject>();
    GameObject selectedMole = null;

    ulong numSavedMoles = 0;
    ulong numDeadMoles = 0;

    public GameObject molePrefab;

    public ParticleSystem confettiParticle;

    // Start is called before the first frame update
    void Start()
    {
        GameData gd = GameDataFileHandler.Load();
        numSavedMoles = gd.molesSaved;
        numDeadMoles = gd.molesDead;

        tileManagerScript = GetComponent<TileManager>();

        if (tileManagerScript.isTileManagerDone == true)
        {
            GenerateMoles();
        }
    }

    // Update is called once per frame
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
                selectedMole.transform.position += new Vector3(-0.1f, 0f);
                //body.velocity += new Vector2(-0.1f, 0f);
            }
            if (Input.GetKey("d"))
            {
                selectedMole.transform.position += new Vector3(0.1f, 0f);
                //body.velocity += new Vector2(0.1f, 0f);
            }
            if (Input.GetKey("w"))
            {
                selectedMole.transform.position += new Vector3(0f, 0.5f);
                //body.velocity += new Vector2(0f, 0.5f);
            }

            // move camera to selected mole position
            mainCamera.transform.position = new Vector3(selectedMole.transform.position.x, selectedMole.transform.position.y, -10);
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
                //plays confetti at the position where the successful mole is for 2 seconds
                //confetti then stops
                confettiParticle.transform.position = mole.transform.position;
                confettiParticle.Play();
                StartCoroutine(StopConfetti(confettiParticle, 2));

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

            Debug.Log("A mole has been saved!");
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

    void GenerateMoles()
    {
        for (int i = 0; i < tileManagerScript.tileTypes.GetLength(0) - 1; i++)
        {
            for (int j = 0; j < tileManagerScript.tileTypes.GetLength(1) - 1; j++)
            {
                if (tileManagerScript.tileTypes[i, j] == TileManager.TileType.Open)
                {
                    //TODO:
                    //figure out why the moles can't align with the empty tiles.
                    GameObject obj = (GameObject)Instantiate(molePrefab, new Vector3((i - 10.5f), (j - 10), -1), Quaternion.identity);
                    moles.Add(obj);
                }
            }
        }
    }

    IEnumerator StopConfetti(ParticleSystem confetti, float time)
    {
        yield return new WaitForSeconds(time);
        confetti.Stop();
    }
}
