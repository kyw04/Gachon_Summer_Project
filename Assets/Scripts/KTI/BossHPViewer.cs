using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    public Slider B_hpbar;
    public GameObject Text;


    public float B_maxHp = 100f; // 보스 최대 체력
    public float B_curHp = 100f; // 보스 현재 체력

    public void Start()
    {
        Text.SetActive(false);
        B_hpbar.value = (float)B_curHp / (float)B_maxHp;
        B_hpbar.minValue = 0;

    }
    public void Update()
    {
        B_hpbar.maxValue = B_maxHp;
        B_hpbar.value = B_curHp;
    }

    public void Fire()
    {
        if (B_hpbar.value > 0)
        {
            B_curHp -= 0.2f;
        }
    }

    public void Sky()
    {
        if (B_hpbar.value > 0)
        {
            B_curHp -= 0.05f;
        }
    }

    public void Boss3()
    {
        if (B_hpbar.value > 0)
        {
            B_curHp -= 10f;
        }

        if (B_hpbar.value <= 0)
        {
            B_hpbar.value = 0;
            Text.SetActive(true);
            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene(3);
            }
        }
    }
}

