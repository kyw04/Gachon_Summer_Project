using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHit : MonoBehaviour
{
    PlayerHPViewer player;
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<PlayerHPViewer>();
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Debug.Log("wow");
            player.Fireball();
        }
    }

}