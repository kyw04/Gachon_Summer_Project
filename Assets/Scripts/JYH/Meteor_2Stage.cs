using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_2Stage : MonoBehaviour
{
    public ObjectPoolComponent boss_Attack;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DestroyLine"))
        {
            boss_Attack.FreeItem(gameObject);
            Debug.Log("123123");
        }
        else if (collision.gameObject.CompareTag("Player"))
            Debug.Log("�÷��̾ ����"); // player���� sendmassage ���
    }
}
