using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Boss_Line : MonoBehaviour
{
    public GameObject boss_Wall;
    public bool isArrive_Boss;
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            boss_Wall.SetActive(true);
            isArrive_Boss = true;
        }
    }
}