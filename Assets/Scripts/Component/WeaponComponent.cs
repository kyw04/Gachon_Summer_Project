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
        var instance = col.gameObject.transform.root.gameObject;
        if (instance.Equals(_owner.gameObject)) return;
        if (instance.CompareTag("Player"))
        {
            var component = instance.GetComponent<PlayerComponent>();
            if (_owner.isAttacking)
            {
                if (component.ModifyHealthPoint(_attackPoint * -1) == -1)
                {
                    _owner.animator.SetTrigger("Dancing");
                }
            }
        }
        else if (instance.CompareTag("Enemy"))
        {
            var component = instance.GetComponent<BattleableComponentBase>();
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
