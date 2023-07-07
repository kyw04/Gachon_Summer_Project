using JYH;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Meteor_2Stage : MonoBehaviour
{
    public Transform Player;

    void Start()
    {
        Player = FindObjectOfType<PlayerCtrl>().transform;
    }


    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Player.SendMessage("Damaged", 0.2f);
            Debug.Log("플레이어가 맞음"); // player에게 sendmassage 사용
        }
    }
}
