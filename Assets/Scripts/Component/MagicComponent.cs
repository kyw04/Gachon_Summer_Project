using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy")) return;
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.root.gameObject.GetComponent<PlayerComponent>().ModifyHealthPoint(-1 * caster.Status.attackPoint);
        }
        Destroy(this.gameObject);
    }
}
