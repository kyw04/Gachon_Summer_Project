using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public GameObject[] zombieObjects; // Zombie 태그를 가진 오브젝트 배열
    public GameObject D_Object;

    private void Start()
    {

    }

    private void Update()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");
        // Zombie 태그를 가진 오브젝트가 전부 사라졌을 때
        if (zombieObjects.Length == 0)
        {
            Destroy(D_Object); // Zombie 전부 사라지면 이벤트 발생
        }
    }
}