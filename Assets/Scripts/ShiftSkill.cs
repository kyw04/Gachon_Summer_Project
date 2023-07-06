using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Xml.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ShiftSkill : MonoBehaviour
{
    public Image img_Skill;
    bool cool = false;
    float time;

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !cool)
        {
            cool = true;
            StartCoroutine(CoolTime(3.0f));
            time = 3;
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



        IEnumerator CoolTime(float cool)
        {
            // Debug.Log("��Ÿ�� �ڷ�ƾ ����");

            while (cool > 1.0f)
            {
                cool -= Time.deltaTime;
                img_Skill.fillAmount = (1.0f / cool);
                yield return new WaitForFixedUpdate();
            }
            // Debug.Log("��Ÿ�� �ڷ�ƾ �Ϸ�");
        }

    }

}
