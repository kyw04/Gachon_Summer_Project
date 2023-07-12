using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField]
    public Slider P_hpbar;
    public Text hp_text;

    public float P_maxHp = 100f; // �÷��̾� �ִ� ü��
    public float P_curHp = 100f; // �÷��̾� ���� ü��




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
            if (P_hpbar.value <= 0)
            {
                //P_hpbar.value = 0;
                //hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());
                SceneManager.LoadScene(4);
            }
    }
    



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
            SceneManager.LoadScene(4);
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
    // �ӽ÷� VŰ�� ������ �� �ǰ� ���̵�Ȥ �� 
    // ���� ���ݿ� �¾��� ���� �ڵ� �����ؾ� ��
}

