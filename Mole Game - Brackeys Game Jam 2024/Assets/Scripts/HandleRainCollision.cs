using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleRainCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject go = collision.gameObject;
        if (go.name == "Rain Drop")
        {
            Debug.Log("Mole hit water!");
            tag = "dead";
        }
    }
}