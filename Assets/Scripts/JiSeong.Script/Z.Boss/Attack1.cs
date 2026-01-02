using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1 : MonoBehaviour
{
    public PlayerComponent player;
    private void Start()
    {
        player = FindObjectOfType<PlayerComponent>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var v = collision.gameObject.GetComponent<PlayerComponent>();
            //v.ModifyHealthPoint(-20);
            player.SendMessage("Damaged",20f);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
