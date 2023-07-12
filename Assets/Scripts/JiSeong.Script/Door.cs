using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform player; // �÷��̾��� Transform ������Ʈ

    public Transform pivotPoint; // ȸ�� �߽���
    public float openAngle = 90f; // ���� ���� ����
    public float rotationSpeed = 60f; // ȸ�� �ӵ�
    public GameObject prisonObject; // ���� ������Ʈ

    private bool isOpen = false; // �� ���� ����

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= 5f)
        {
            isOpen = true;
            Destroy(prisonObject);
        }

        if (isOpen)
        {
            float targetAngle = 0f;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, -rotationSpeed * Time.deltaTime);
        }
    }
    
}
