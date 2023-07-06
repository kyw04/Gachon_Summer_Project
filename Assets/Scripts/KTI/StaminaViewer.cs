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
    // float time;


    private void Start()
    {
        stbar.value = (float)curST / (float)maxST;

        enable = true;
    }


    private void Update()
    {


        stbar.value += 0.33f * Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.LeftShift) && enable)
        {
            StartCoroutine(ENABLE());
            stbar.value -= 1.0f;
        }

   
    }
    IEnumerator ENABLE()
    {
        enable = false;
        yield return new WaitForSeconds(3);
        enable = true;

    }
}
