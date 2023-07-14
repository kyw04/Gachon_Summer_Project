using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class SkyEvasion : MonoBehaviour
{
    BossHPViewer boss1;

    private void Start()
    {
        boss1 = GameObject.Find("BossHPSlider").GetComponent<BossHPViewer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
        }
        if (collision.gameObject.tag == "Bottom")
        {
            Destroy(gameObject);
            boss1.Sky();
        }
 
        if (collision.gameObject.tag == "Wall")
        {
            Destroy(gameObject);
            boss1.Sky();
        }
        if (collision.gameObject.tag == "Fire")
        {
            Destroy(gameObject);
            boss1.Sky();
        }
        if (collision.gameObject.tag == "Big")
        {
            Destroy(gameObject);
        }
    }
}




