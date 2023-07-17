using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleCollision : MonoBehaviour
{
    public Dragon dragon;

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("Player"))
        {
            dragon.OnDamage();
        }
    }
}
