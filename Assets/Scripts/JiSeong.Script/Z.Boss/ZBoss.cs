using UnityEngine;

public class ZBoss : MonoBehaviour
{
    public Transform target; // �÷��̾��� Transform ������Ʈ

    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float rotationSpeed = 5f; // ȸ�� �ӵ�
    public float attackRange = 2f; // ���� ����
    public float attackDamage = 10f; // ���ݷ�
    public float attackCooldown = 2f; // ���� ��ٿ�
    public float pushForce = 5f; // �浹�� ���� �и��� ��
    public int EnemyHp = 3;
    public float ignoreCollisionDuration = 3f; // �浹 ���� �Ⱓ

    private Animator animator; // Animator ������Ʈ
    private bool canAttack = true; // ���� ���� ����
    private bool isAttacking = false; // ���� �� ����
    private Rigidbody rb; // Rigidbody ������Ʈ
    private float ignoreCollisionTimer; // �浹 ���� Ÿ�̸�
    private bool isDying = false;
    private Renderer objectRenderer;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator ������Ʈ ��������
        rb = GetComponent<Rigidbody>(); // Rigidbody ������Ʈ ��������
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (target != null)
        {
            if (EnemyHp > 0)
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

                        // �ִϸ��̼� ���
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed �Ķ���� ����
                        }
                    }
                }
            }
        }
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Z_Attack"))
        {
            animator.ResetTrigger("isAttack");
        }
        if (ignoreCollisionTimer > 0f)
        {
            ignoreCollisionTimer -= Time.deltaTime;
        }
    }
    public void Die()
    {
        if (!isDying)
        {
            isDying = true;
            StartCoroutine(DieCoroutine());
        }
    }
    private System.Collections.IEnumerator DieCoroutine()
    {
        yield return new WaitForSeconds(5f);

        // 4�� �ڿ� ������Ʈ ������� �ϱ�
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // �浹�� �߻��� ���
        if (collision.gameObject.CompareTag("Player"))
        {

            ignoreCollisionTimer = ignoreCollisionDuration;
            StartCoroutine(ResetIgnoreCollisionTimer());
        }
    }
    private System.Collections.IEnumerator ResetIgnoreCollisionTimer()
    {
        // HP�� ���ҽ�Ŵ
        EnemyHp--;

        if (EnemyHp <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
            rb.isKinematic = true;
            animator.SetTrigger("isDie");
            Die();
        }

        yield return new WaitForSeconds(ignoreCollisionDuration);

        ignoreCollisionTimer = 0f;
    }
    private void Attack()
    {
        if (target != null)
        {
            // �÷��̾� �������� ȸ��
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // ���� �������� ���� (y�� ȸ�� ����)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            // �ִϸ��̼� ���
            if (animator != null)
            {
                animator.SetTrigger("isAttack"); // Attack Ʈ���� ����
            }

            // �÷��̾�� ���ݷ��� ���� �������� ����


            // ���� ��ٿ� ����
            canAttack = false;
            isAttacking = true;
            Invoke("ResetAttack", attackCooldown);
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
    }
}
