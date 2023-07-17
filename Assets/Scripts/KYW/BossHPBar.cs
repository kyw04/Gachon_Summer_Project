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
        float hp = boss.health / boss.Status.maxHealthPoint;
        hpSlider.value = hp;
        hpText.text = boss.health.ToString("F0");
    }
}
