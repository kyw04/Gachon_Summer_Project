using UnityEngine;

public class Door : MonoBehaviour
{
    public Transform player; // 플레이어의 Transform 컴포넌트

    public Transform pivotPoint; // 회전 중심점
    public float openAngle = 90f; // 열린 상태 각도
    public float rotationSpeed = 60f; // 회전 속도
    public GameObject prisonObject; // 감옥 오브젝트

    private bool isOpen = false; // 문 열림 여부

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
