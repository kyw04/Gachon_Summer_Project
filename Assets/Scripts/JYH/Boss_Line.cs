using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Boss_Line : MonoBehaviour
{
    public Stage2_Boss boss;
    public GameObject boss_Wall;
    public bool isArrive_Boss;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.BGM_Sound(1);
            boss_Wall.SetActive(true);
            isArrive_Boss = true;
            boss.SendMessage("Health_Bar");
        }
    }
}