using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class RainManager : MonoBehaviour
{
    public ParticleSystem rainParticle;
    public List<ParticleCollisionEvent> rainCollisions;

    void Start()
    {
        rainParticle = GetComponent<ParticleSystem>();
        rainCollisions = new List<ParticleCollisionEvent>();
    }

    //TODO:
    //figure out why this isn't working
    void OnParticleCollision(GameObject other)
    {
        int numberOfCollisions = rainParticle.GetCollisionEvents(other, rainCollisions);

        Rigidbody rigidbody = other.GetComponent<Rigidbody>();
        int i = 0;

        while (i < numberOfCollisions)
        {
            if (rigidbody)
            {
                if (other.name == "Mole")
                {
                    Debug.Log("Mole hit water!");
                    other.tag = "dead";
                }
            }

            i++;
        }
    }
}