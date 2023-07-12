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
    public GameObject Text2;
    public GameObject Door;
    public GameObject Bottom;
    public GameObject BigFire;
    public GameObject Map;


    public float B_maxHp = 100f; // 보스 최대 체력
    public float B_curHp = 100f; // 보스 현재 체력


    bool Actived = false;
    public void Start()
    {
        Text.SetActive(false);
        Text2.SetActive(false);
        Door.SetActive(true);
        Bottom.SetActive(true);
        BigFire.SetActive(false);
        Map.SetActive(false);
        
        
        Invoke("Text2Fade", 5f);
        B_hpbar.value = (float)B_curHp / (float)B_maxHp;
        B_hpbar.minValue = 0;

    }
    public void Update()
    {
        B_hpbar.maxValue = B_maxHp;
        B_hpbar.value = B_curHp;

        if (B_hpbar.value <= 50f && !Actived)
        {
            Actived = true;
            Text2.SetActive(true);
            Door.SetActive(false);
            Map.SetActive(true);
            BigFire.SetActive(true);
            StartCoroutine(HideText());
            StartCoroutine(HideMap());
            
        }  
    }

    public void Fire()
    {
        if (B_hpbar.value > 0)
        {
            B_curHp -= 0.05f;
        }
    }

    public void Sky()
    {
        if (B_hpbar.value > 0)
        {
            B_curHp -= 0.008f;
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
    IEnumerator HideText()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            Text2.SetActive(false);
        }
    }

    IEnumerator HideMap()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            Door.SetActive(true);
            Bottom.SetActive(false);
            Map.SetActive(false);
        }
    }

}

