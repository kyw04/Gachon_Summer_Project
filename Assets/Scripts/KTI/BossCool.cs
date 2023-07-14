using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BossCool : MonoBehaviour
{
    public Image img_Skill;
    bool cool = true;
    float time;

    private void Update()
    {
        if (!cool)
        {

            cool = true;
            StartCoroutine(CoolTime(6.0f));
            time = 5;
            //  Debug.Log("cool");

        }

        if (time > 0)
        {
            time -= Time.deltaTime;
        }
        else if (time <= 0)
        {
            cool = false;
            //  Debug.Log("End");
        }

    }




    IEnumerator CoolTime(float cool)
    {
        while (cool > 1.0f)
        {
            cool -= Time.deltaTime;
            img_Skill.fillAmount = (1.0f / cool);
            yield return new WaitForFixedUpdate();
        }
    }



}


