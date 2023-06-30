using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform ������Ʈ

    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public float attackRange = 2f; // ���� ����
    public float attackDamage = 10f; // ���ݷ�
    public float attackCooldown = 2f; // ���� ��ٿ�

    private Animator animator; // Animator ������Ʈ
    private bool canAttack = true; // ���� ���� ����
    private bool isAttacking = false; // ���� �� ����

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
    }

    private void Update()
    {
        if (target != null)
        {
            // �÷��̾� ���� ���ϱ�
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            direction.Normalize(); // ���͸� ���� ���ͷ� ����ȭ

            // ���� �÷��̾� ������ �Ÿ� ���
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= attackRange && canAttack && !isAttacking)
            {
                // �÷��̾ ���� ���� ���� �ְ� ���� �����ϸ� ���� ���� �ƴ� ���
                Attack();
                print("����");
            }
            else
            {
                if (distance >= attackRange)
                {
                    // �� �̵�
                    transform.position += direction * moveSpeed * Time.deltaTime;

                    // �� ȸ��
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                }
            }
        }
    }

    private void Attack()
    {
        // �÷��̾� �������� ȸ��
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // �ִϸ��̼� ���
        animator.SetTrigger("isAttack"); // isAttack Ʈ���� ����

        // ���� ��ٿ� ����
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
        print("�ʱ�ȭ");
    }
}
