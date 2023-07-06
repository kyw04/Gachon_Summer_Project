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

    private float B_maxHp = 100f; // 보스 최대 체력
    private float B_curHp = 100f; // 보스 현재 체력



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
    // 임시로 B키를 눌렀을 때 피가 깎이도혹 함 
    // 플레이어의 공격에 맞았을 때로 코드 수정해야 함

}          

