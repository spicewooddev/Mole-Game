using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class CameraOperations : MonoBehaviour
{
    public int panSpeed = 10;

    private Vector3 mouseOrigin;

    //private bool isMouseDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        MouseInputs();
        ButtonInputs();
    }

    void MouseInputs()
    {
        //when the left-mouse button is PRESSED, this happens
        if (Input.GetMouseButtonDown(0))
        {
            mouseOrigin = Input.mousePosition;
        }

        //when the left-mouse button is HELD DOWN, this happens
        if (Input.GetMouseButton(0))
        {
            Vector3 delta = Input.mousePosition - mouseOrigin;

            transform.position += new Vector3(-delta.x, -delta.y) * Time.deltaTime;

            mouseOrigin = Input.mousePosition;
        }
    }

    void ButtonInputs()
    {
        /*
        Vector3 pos = transform.position;
        if (Input.GetKey("left"))
        {
            pos.x -= 0.2f; 
        }
        
        if (Input.GetKey("right"))
        {
            pos.x += 0.2f; 
        }

        if (Input.GetKey("up"))
        {
            pos.y += 0.2f; 
        }
        
        if (Input.GetKey("down"))
        {
            pos.y -= 0.2f; 
        }
        */

        //transform.position = pos;

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
