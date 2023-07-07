using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    private Slider B_hpbar;

    public GameObject Text;

    public Text text;

    private float B_maxHp = 100f; // ���� �ִ� ü��
    private float B_curHp = 100f; // ���� ���� ü��



    void Start()
    {
        Text.SetActive(false);

        B_hpbar.value = (float) B_curHp / (float) B_maxHp;      

           
    }

  
         

     
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (B_curHp > 0)
            {
                B_hpbar.value -= 0.1f;
                
            }
            
        }
        if (B_hpbar.value <= 0)
        {
            Text.SetActive(true);

            if (Input.GetKeyDown(KeyCode.N))
            {
                SceneManager.LoadScene(3);
            }
        }
    }
    // �ӽ÷� BŰ�� ������ �� �ǰ� ���̵�Ȥ �� 
    // �÷��̾��� ���ݿ� �¾��� ���� �ڵ� �����ؾ� ��

}          

