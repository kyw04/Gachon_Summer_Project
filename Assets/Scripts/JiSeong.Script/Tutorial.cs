using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] zombieObjects; // Zombie �±׸� ���� ������Ʈ �迭
    public Transform pivotPoint; // ȸ�� �߽���
    public float openAngle = 90f; // ���� ���� ����
    public float rotationSpeed = 60f; // ȸ�� �ӵ�
    private bool isOpen = false; // �� ���� ����
    private bool spawn = false;
    public Transform spawnPoint; // ������Ʈ�� ��ȯ�� ��ġ

    private void Start()
    {

    }

    private void Update()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        // Zombie �±׸� ���� ������Ʈ�� ���� ������� ��
        if (zombieObjects.Length == 0)
        {
            isOpen = true;
        }
        if (isOpen)
        {
            float targetAngle = 0f;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, -rotationSpeed * Time.deltaTime);
        }
    }
}