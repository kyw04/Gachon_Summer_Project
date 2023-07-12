using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireGround : MonoBehaviour
{
    //PlayerHPViewer playerHP;
    void Start()
    {
    //    playerHP = GameObject.Find("Player").GetComponent<PlayerHPViewer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
        //    playerHP.Ground();
        }
    }
}
