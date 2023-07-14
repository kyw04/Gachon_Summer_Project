using UnityEngine;

public class Attack2 : MonoBehaviour
{
    public float speed = 10f; // 발사체의 속도

    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // 발사체를 일정한 속도로 앞으로 이동시킴
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
    void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생하면 오브젝트 삭제
        Destroy(gameObject);
    }
}