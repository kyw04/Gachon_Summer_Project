using UnityEngine;

public class Attack2 : MonoBehaviour
{
    public float speed = 10f; // �߻�ü�� �ӵ�

    private Rigidbody rb;

    private void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // �߻�ü�� ������ �ӵ��� ������ �̵���Ŵ
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
    void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻��ϸ� ������Ʈ ����
        Destroy(gameObject);
    }
}