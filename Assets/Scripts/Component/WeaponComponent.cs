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
        if (col.gameObject.CompareTag("Player"))
        {
            Debug.Log(col.gameObject.name);
            var component = col.transform.root.GetComponent<PlayerComponent>();
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
