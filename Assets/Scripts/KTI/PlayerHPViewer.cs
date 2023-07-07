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

    private float P_maxHp = 100f; // 플레이어 최대 체력
    private float P_curHp = 100f; // 플레이어 현재 체력

    


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
    // 임시로 V키를 눌렀을 때 피가 깎이도혹 함 
    // 적의 공격에 맞았을 때로 코드 수정해야 함

}

