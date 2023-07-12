using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
[RequireComponent(typeof(Projector))]
public class MagicComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public BattleableComponentBase caster;
    public Vector3 spawnOffset;
    public GameObject explodePrefab;

    protected virtual void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy")) return;
        if (other.gameObject.CompareTag("Player"))
        {
            other.transform.root.gameObject.GetComponent<PlayerComponent>().ModifyHealthPoint(-1 * caster.Status.attackPoint);
        }

        Instantiate(explodePrefab).transform.position += other.gameObject.CompareTag("Player") ?  other.transform.position  : this.transform.position;
        this.gameObject.SetActive(false);
    }
}
