using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3 : MonoBehaviour
{
    Rigidbody rigid;
    BoxCollider boxCollider;
    BossHPViewer boss3;

    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("att");
        if(other.tag == "Boss") 
        {
            boss3.Boss3();
        }
    }
}
