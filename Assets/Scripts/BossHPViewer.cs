using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class BossHPViewer : MonoBehaviour
{
    [SerializeField]
    private Slider hpbar;

    private float maxHp = 100;
    private float curHp = 100;

    void Start()
    {
        hpbar.value = (float) curHp / (float) maxHp;   
    }

    //void Update()
    //{
    //    if (/* �÷��̾��� ���ݿ� ������ �¾��� ���*/) 
    //    {
    //        if (curHp > 0)
    //        {
    //            // curHP -= PlayerDamage
    //        }
    //        else
    //        {
    //            curHp = 0;
    //        }
    //    }
    //} 

}
