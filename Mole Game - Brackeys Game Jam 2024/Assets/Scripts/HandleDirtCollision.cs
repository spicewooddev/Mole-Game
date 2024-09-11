using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandleDirtCollision : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collider)
    {
        GameObject rainDrop = collider.gameObject;
        Rigidbody2D body = rainDrop.GetComponent<Rigidbody2D>();
        body.velocity *= 0.1f;
    }
}