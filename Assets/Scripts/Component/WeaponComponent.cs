using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Collider))]
public class WeaponComponent : MonoBehaviour
{
    private BattleableComponentBase _owner;
    private int _attackPoint;

    private void Start()
    {
        _owner = this.transform.root.GetComponent<BattleableComponentBase>();
    }

    private void Update()
    {
        _attackPoint = _owner.Status.attackPoint;
    }

    private void OnCollisionEnter(Collision col)
    {
        //if(col.gameObject.CompareTag(_owner.gameObject.tag)) return;
        
        Debug.Log(col.gameObject.tag + " " + col.gameObject.name);
        if (col.gameObject.CompareTag("Player"))
        {
            var component = col.transform.root.GetComponent<PlayerComponent>();
            if (_owner.isAttacking)
            {
                if (component.ModifyHealthPoint(_attackPoint * -1) == -1)
                {
                    _owner.animator.SetTrigger("Dancing");
                }
            }
        }
        else if (col.gameObject.CompareTag("Enemy"))
        {
            var component = col.transform.root.GetComponent<BattleableComponentBase>();
            if (_owner.isAttacking)
            {
                if (component.ModifyHealthPoint(_attackPoint * -1) == -1)
                {
                    _owner.animator.SetTrigger("Dancing");
                }
            }
        }
    }
}