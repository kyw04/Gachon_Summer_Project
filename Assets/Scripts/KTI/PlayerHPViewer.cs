using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;


public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField]
    public Slider P_hpbar;
    public Text hp_text;

    public float P_maxHp = 100f; // 플레이어 최대 체력
    public float P_curHp = 100f; // 플레이어 현재 체력




    public void Start()
    {
        P_hpbar.value = (float)P_curHp / (float)P_maxHp;
        P_hpbar.minValue = 0;
    }
    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Enemy")
        {
            if (P_curHp > 0)
            {
                P_curHp -= 10f;
                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            }
        }

        if (collision.gameObject.tag == "Fireball")
        {
            if (P_curHp > 0)
            {
                P_curHp -= 1f;
                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            }
        }

        if (collision.gameObject.tag == "Magic")
        {
            if (P_curHp > 0)
            {
                P_curHp -= 2f;
                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            }
        }
        if (collision.gameObject.tag == "Big")
        {
            if (P_curHp > 0)
            {
                P_curHp -= 70f;
                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            }
        }
        if (collision.gameObject.tag == "Heal")
        {
            if (P_curHp > 0)
            {
                P_curHp += 10f;
                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            }
        }

        if (P_hpbar.value <= 0)
        {
            //P_hpbar.value = 0;
            //hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            SceneManager.LoadScene(4);
        }
    }



    //        if (collision.gameObject.tag == "Enemy")
    //        {
    //            if (P_curHp > 0)
    //            {
    //                P_curHp -= 10f;
    //                hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
    //            }
    //        }
    //        if (P_hpbar.value <= 0)
    //        {
    //            //P_hpbar.value = 0;
    //            //hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
    //            SceneManager.LoadScene(4);
    //        }
    //    }


    public void Update()
    {
        P_hpbar.maxValue = P_maxHp;
        P_hpbar.value = P_curHp;

        /* if (Input.GetKeyDown(KeyCode.V))
         {
             if (P_curHp > 0)
             {
                 P_curHp -= 13.0f;
                 hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
             }
         }
         if (P_hpbar.value <= 0)
         {
             P_hpbar.value = 0;
             hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
           //  SceneManager.LoadScene(4);
         } */
    }
    public void Ground()
    {
        if (P_hpbar.value > 0)
        {
            P_curHp -= 0.5f;
            hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
        }
        if (P_hpbar.value <= 0)
        {
            //P_hpbar.value = 0;
            //hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
            // SceneManager.LoadScene(4);
        }
    }

    public void Fireball()
    {
        if (P_hpbar.value > 0)
        {
            P_curHp -= 10f;
            hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
        }
    }

    public void Heal()
    {
        if (P_hpbar.value > 0)
        {
            P_curHp += 10f;
            hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
        }
        if (P_hpbar.value > 90)
        {
            P_curHp = 100f;
            hp_text.text = (100 + "/" + 100);
        }
    }

    void OnTriggerEnter(Collider other)
        {
            switch (other.gameObject.tag)
            {
                case "Meteor":
                    //        Player.SendMessage("Damaged", 0.2f);
                    break;
                case "CircleMeteor":
                    //        Player.SendMessage("Damaged", 0.2f);
                    break;
            }
        }
        // 임시로 V키를 눌렀을 때 피가 깎이도혹 함 
        // 적의 공격에 맞았을 때로 코드 수정해야 함
 }



