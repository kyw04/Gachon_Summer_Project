//플레이어와 같이 키보드 등으로 조작이 가능한 오브젝트일 경우 구현해야 하는 메소드를 명시

using Unity.VisualScripting;

public interface IControllable
{
    public bool isControllable { get; set; }
    public void Command();
}
