//"플레이어 오브젝트" 의 데이터를 담을 VO
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
    }

    public PlayerVO(string name, int maxHealthPoint, int maxStaminaPoint, int attackPoint, int defencePoint, float spd)
    {
        base.name = name;
        base.maxHealthPoint = maxHealthPoint;
        base.maxStaminaPoint = maxStaminaPoint;
        base.attackPoint = attackPoint;
        base.defencePoint = defencePoint;
        base.spd = spd;
    }
}