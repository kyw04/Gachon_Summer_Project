//전투 가능한 오브젝트들의 데이터를 담는 vo 클래스
using System;

[Serializable]
public class BattleableVOBase
{
    public string name = "John";
    public int maxHealthPoint = 0;
    public int maxStaminaPoint = 0;
    public int attackPoint = 0;
    public int defencePoint = 0;
    public float spd = 4;
}