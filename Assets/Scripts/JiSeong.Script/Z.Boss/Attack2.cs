using UnityEngine;

public class Attack2 : MonoBehaviour
{
    public float speed = 10f; // �߻�ü�� �ӵ�
    private Rigidbody rb;
    public PlayerComponent player;

    private void Start()
    {
        // Rigidbody ������Ʈ ��������
        rb = GetComponent<Rigidbody>();
        player = FindObjectOfType<PlayerComponent>();

    }

    private void Update()
    {
        // �߻�ü�� ������ �ӵ��� ������ �̵���Ŵ
        Vector3 movement = transform.forward * speed * Time.deltaTime;
        rb.MovePosition(rb.position + movement);
    }
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var v = collision.gameObject.GetComponent<PlayerComponent>();
            //v.ModifyHealthPoint(-20);
            player.SendMessage("Damaged", 20f);
        }
        Destroy(gameObject);
    }
}