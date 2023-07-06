using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    private Slider hpbar;

    private float maxHp = 100f;
    private float curHp = 100f;

    void Start()
    {
        hpbar.value = (float) curHp / (float) maxHp;
    }

    //void Update()
    //{
    //    if (플레이어의 공격에 보스가 맞았을 경우) 
    //    {
    //        if (curHp > 0)
    //        {
    //            // curHP -= Damage
    //        }
    //        else
    //        {
    //            curHp <= 0;
    //        }
    //    }
    // 
    //   
    //      
    //    
} 

