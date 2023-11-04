using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Effect : MonoBehaviour
{
    public int Atk
    {
        get;
        set;
    }

    private ParticleSystem particleSystem;
    public Collider collider;

    private void Start()
    {
        particleSystem = GetComponent<ParticleSystem>();
        collider = GetComponent<Collider>();
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(Atk);
            }
        }
    }
}
