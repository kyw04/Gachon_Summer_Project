using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float attackRange = 0.5f; // ���� ����
    public int damage = 10; // ���ݷ�

    private Transform player; // �÷��̾��� ��ġ
    private Animator animator; // �ִϸ����� ������Ʈ

    private bool isAttacking = false; // ���� ���� ������ ����

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // �÷��̾��� �±װ� "Player"�� ������Ʈ�� ã�Ƽ� ��ġ�� ������
        animator = GetComponent<Animator>(); // �ִϸ����� ������Ʈ ��������
    }

    private void Update()
    {
        if (player == null)
        {
            return; // �÷��̾ ������ �������� ����
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            // �÷��̾ ���� �̵�
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        // �÷��̾���� �Ÿ��� ���� ���� �̳��� ��� ����
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
            animator.SetTrigger("Attack");
            isAttacking = true;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0; // ���� ���⸸ ����ϰ� y�� ȸ���� ����
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void AttackPlayer()
    {
        
    }
}