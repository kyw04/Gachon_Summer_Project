using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] zombieObjects; // Zombie �±׸� ���� ������Ʈ �迭
    public GameObject D_Object;

    private void Start()
    {

    }

    private void Update()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        // Zombie �±׸� ���� ������Ʈ�� ���� ������� ��
        if (zombieObjects.Length == 0)
        {
            Destroy(D_Object); // Zombie ���� ������� �̺�Ʈ �߻�
        }
    }
}