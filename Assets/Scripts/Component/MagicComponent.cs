using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MagicComponent : MonoBehaviour
{
    // Start is called before the first frame update
    public BattleableComponentBase caster;
    private void Awake()
    {
        this.transform.position += new Vector3(0, 10, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log(other.gameObject.tag + " " + other.gameObject.name);
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerComponent>().ModifyHealthPoint(-1 * caster.Status.attackPoint);
        }
        Destroy(this.gameObject);
    }
}
