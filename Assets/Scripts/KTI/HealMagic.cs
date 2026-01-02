using JiSeong;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealMagic : MonoBehaviour
{
    PlayerHPViewer playerHP;

    private void Start()
    {
        playerHP = GameObject.Find("Player").GetComponent<PlayerHPViewer>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            playerHP.Heal();
        }
    }
}
