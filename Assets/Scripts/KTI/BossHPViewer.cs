using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    private Slider B_hpbar;
    public GameObject Text;

    private float B_maxHp = 100f; // 보스 최대 체력
    private float B_curHp = 100f; // 보스 현재 체력

    void Start()
    {
        Text.SetActive(false);
        B_hpbar.value = (float)B_curHp / (float)B_maxHp;
        B_hpbar.minValue = 0;
    }

    private void Update()
    {
        B_hpbar.maxValue = B_maxHp;
        B_hpbar.value = B_curHp;
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            if (B_curHp > 0)
            {
                B_curHp -= 10f;
                Debug.Log("!");
                //Destroy(gameObject);
            }
          
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

