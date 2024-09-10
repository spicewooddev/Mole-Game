using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDirtCollision : MonoBehaviour
{
    void OnTiggerEnter(Collider collider)
    {
        Debug.Log("Toucing dirt!");
    }
}
