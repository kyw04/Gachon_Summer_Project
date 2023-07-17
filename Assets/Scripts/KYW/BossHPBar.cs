using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHPBar : MonoBehaviour
{
    public Dragon boss;
    public Slider hpSlider;
    public Text hpText;

    private void Update()
    {
        hpSlider.value = boss.health.Value / (float)boss.Status.maxHealthPoint;
        hpText.text = boss.health.Value.ToString("F0");
    }
}
