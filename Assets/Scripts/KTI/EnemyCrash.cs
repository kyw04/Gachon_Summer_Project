using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class EnemyCrash : MonoBehaviour
{
    [SerializeField]
    private Slider P_hpbar;
    public Text hp_text;

    private float P_maxHp = 100f; // 플레이어 최대 체력
    private float P_curHp = 100f; // 플레이어 현재 체력

    private void Start()
    {
        P_hpbar.value = (float)P_curHp / (float)P_maxHp;
    }
    

    void TakeDamage (int value)
    {
       
        if (P_curHp < 0) 
        {
            P_curHp -= 10.0f;
        }
    }

    void TakeDamage(float ratio)
    {
        
        if (P_curHp <= 0) 
        {
            SceneManager.LoadScene(4);
        }
    }
    void Die()
    {
        Destroy(this.gameObject);

    }

  

    public void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy"))
        {
            P_curHp -= 10;
            Destroy(collision.gameObject);
        }
    }
}
