//"플레이어 오브젝트" 의 데이터를 담을 VO
public class PlayerVO : BattleableVOBase
{
    public PlayerVO()
    {
        name = "John";
        maxHealthPoint = 100;
        attackPoint = 10;
        defencePoint = 5;
        spd = 10;
    }

    public PlayerVO(string name, int maxHealthPoint, int attackPoint, int defencePoint, float spd)
    {
        base.name = name;
        base.maxHealthPoint = maxHealthPoint;
        base.attackPoint = attackPoint;
        base.defencePoint = defencePoint;
        base.spd = spd;
    }
}