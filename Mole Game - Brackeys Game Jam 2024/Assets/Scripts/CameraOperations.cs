using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOperations : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = transform.position;
        if (Input.GetKey("left"))
        {
            pos.x -= 0.5f; 
        }
        
        if (Input.GetKey("right"))
        {
            pos.x += 0.5f; 
        }

        if (Input.GetKey("up"))
        {
            pos.y += 0.5f; 
        }
        
        if (Input.GetKey("down"))
        {
            pos.y -= 0.5f; 
        }

        transform.position = pos;

        if (Input.GetKey("-"))
        {
            Camera camera = GetComponent<Camera>();
            float size = camera.orthographicSize;
            camera.orthographicSize = Mathf.Clamp(size + 0.1f, 0.5f, 15f);
        }

        if (Input.GetKey("="))
        {
            Camera camera = GetComponent<Camera>();
            float size = camera.orthographicSize;
            camera.orthographicSize = Mathf.Clamp(size - 0.1f, 0.5f, 15f);
        }
    }
}
