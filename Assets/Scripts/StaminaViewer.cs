using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Timers;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class StaminaViewer : MonoBehaviour
{
    [SerializeField]
    private Slider stbar;

    private float maxST = 100;
    private float curST = 100;

    bool enable;
    float time;


    private void Start()
    {
        stbar.value = (float)curST / (float)maxST;

        enable = true;
    }


    private void Update()
    {


        stbar.value += 0.1f * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && enable)
        {
            StartCoroutine(ENABLE());
            stbar.value -= 0.1f;

            /*
            cool = true;
            StartCoroutine(CoolTime(3.0f));
           time = 3;
            curST -= 3; 
            */
        }

        /*
        if (time > 0)
        {
           time -= Time.deltaTime;
        }
        else if (time <= 0)
        {
            cool = false;
            //  Debug.Log("End");

            HandleST();
        }
       void HandleST()
        {
            stbar.value = (float)curST / (float)maxST;
        }

        
        IEnumerator CoolTime(float st)
        {
            //Debug.Log("쿨타임 코루틴 실행");

            while (st > 1.0f)
            {
                st -= Time.deltaTime;
                sl.value = (1.0f / st);
                yield return new WaitForFixedUpdate();
            }
            // Debug.Log("쿨타임 코루틴 완료");
        }*/
        
    }
    IEnumerator ENABLE()
    {
        enable = false;
        yield return new WaitForSeconds(3);
        enable = true;

    }
}
