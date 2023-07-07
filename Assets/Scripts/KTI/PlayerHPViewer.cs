using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerHPViewer : MonoBehaviour
{
    [SerializeField]
    private Slider P_hpbar;
    public Text hp_text;

    private float P_maxHp = 100f; // �÷��̾� �ִ� ü��
    private float P_curHp = 100f; // �÷��̾� ���� ü��

    


    void Start()
    {
       P_hpbar.value = (float)P_curHp / (float)P_maxHp;
       P_hpbar.minValue = 0;
    }





    private void Update()
    {
        P_hpbar.maxValue = P_maxHp;
        P_hpbar.value = P_curHp;
        hp_text.text = (P_curHp.ToString() + "/" + P_maxHp.ToString());

        if (Input.GetKeyDown(KeyCode.V))
        {
            if (P_curHp > 0)
            {
                P_curHp -= 13.0f;
            }

        }
        if (P_hpbar.value <= 0)
        {     
                SceneManager.LoadScene(4);  
        }
    }
    // �ӽ÷� VŰ�� ������ �� �ǰ� ���̵�Ȥ �� 
    // ���� ���ݿ� �¾��� ���� �ڵ� �����ؾ� ��

}

