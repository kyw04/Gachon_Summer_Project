using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public BattleableComponentBase caster;
    public Vector3 spawnOffset; 
    private void Awake()
    {
        this.transform.position += spawnOffset;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerComponent>().ModifyHealthPoint(-1 * caster.Status.attackPoint);
        }
        Destroy(this.gameObject);
    }
}
