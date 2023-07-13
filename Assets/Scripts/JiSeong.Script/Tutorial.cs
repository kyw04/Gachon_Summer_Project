using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] zombieObjects; // Zombie 태그를 가진 오브젝트 배열
    public Transform pivotPoint; // 회전 중심점
    public float openAngle = 90f; // 열린 상태 각도
    public float rotationSpeed = 60f; // 회전 속도
    private bool isOpen = false; // 문 열림 여부
    private bool spawn = false;
    public Transform spawnPoint; // 오브젝트를 소환할 위치

    private void Start()
    {

    }

    private void Update()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        // Zombie 태그를 가진 오브젝트가 전부 사라졌을 때
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