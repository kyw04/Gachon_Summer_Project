using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Meteor_2Stage : MonoBehaviour
{
    PlayerComponent Player;

    private void Start()
    {
        Player = GameObject.Find("Player").GetComponent<PlayerComponent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player.ModifyHealthPoint(-20);
            Player.SendMessage("Damaged", 20f);
        }
    }
}
