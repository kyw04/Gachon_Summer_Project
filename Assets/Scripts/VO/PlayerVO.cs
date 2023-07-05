//"플레이어 오브젝트" 의 데이터를 담을 VO
using UnityEngine;

public class PlayerVO : BattleableVOBase
{
    public PlayerVO()
    {
        name = "John";
        maxHealthPoint = 100;
        maxStaminaPoint = 20;
        attackPoint = 10;
        defencePoint = 5;
        spd = 4;
        position = new Vector3(0, 0, 0);
    }

    public PlayerVO(int id, string name, int maxHealthPoint, int maxStaminaPoint, int attackPoint, int defencePoint, float spd, Vector3 position)
    {
        this.id = id;
        this.name = name;
        this.maxHealthPoint = maxHealthPoint;
        this.maxStaminaPoint = maxStaminaPoint;
        this.attackPoint = attackPoint;
        this.defencePoint = defencePoint;
        this.spd = spd;
        this.position = position;
    }
}