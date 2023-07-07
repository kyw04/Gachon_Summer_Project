using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteor_2Stage : MonoBehaviour
{
    public ObjectPoolComponent boss_Attack;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("DestroyLine"))
            boss_Attack.FreeItem(gameObject);
        else if (collision.gameObject.CompareTag("Player"))
            Debug.Log("플레이어가 맞음"); // player에게 sendmassage 사용
    }
}
