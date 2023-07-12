// 전투 가능한 오브젝트들이 가져야할 기능을 명시하는 인터페이스
public interface IBattleable
{
    public int healthPoint { get; set; }
    public void Attack();
    public int ModifyHealthPoint(int amount);
    public void Die();

    public void Move();
}