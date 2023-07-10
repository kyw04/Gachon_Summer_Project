using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Bullet : MonoBehaviour
{
    public int damdge;
    public bool isMelee;
    public bool isBall;

    private void OnCollisionEnter(Collision collision)
    {
        if(!isBall && collision.gameObject.tag == "Floor") 
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isMelee && other.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
        }
    }



}
