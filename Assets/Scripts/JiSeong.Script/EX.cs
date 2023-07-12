using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target; // Ÿ�� = �÷��̾�

    public float moveSpeed = 5f; // ���� �̵� �ӵ�
    public float rotationSpeed = 5f; // �÷��̾ �ٶ� �� ȸ�� �ӵ�
    public float attackRange = 2f; // ���� ���� ����
    public float attackCooldown = 2f; // ���� ��ٿ�
    public float knockbackForce = 10f; // �ڷ� �з����� ��
    public float maxDistance = 8f; // �÷��̾���� �ִ� ��� �Ÿ�

    private Animator animator; // �ִϸ����� ������Ʈ
    private bool canAttack = true; // ���� ���� ����
    private bool isAttacking = false; // ���� �� ����
    private Rigidbody rb; // Rigidbody ������Ʈ

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (target != null)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, target.position);
            if (distanceToPlayer <= maxDistance)
            {
                Vector3 direction = target.position - transform.position;
                direction.y = 0;
                direction.Normalize();
                float distance = Vector3.Distance(transform.position, target.position);
                if (distance <= attackRange && canAttack && !isAttacking)
                {
                    Attack();
                }
                else
                {
                    transform.position += direction * moveSpeed * Time.deltaTime;
                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                    if (animator != null)
                    {
                        animator.SetFloat(MoveSpeedHash, direction.magnitude); // MoveSpeed �Ķ���� ����
                    }
                }
            }
        }
    }

    private void Attack()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;
            if (animator != null)
            {
                animator.SetTrigger(AttackTriggerHash); 
            }
            canAttack = false;
            isAttacking = true;
            Invoke("ResetAttack", attackCooldown);
            if (rb != null)
            {
                Vector3 knockbackDirection = -direction.normalized;
                rb.AddForce(knockbackDirection * knockbackForce, ForceMode.Impulse);
            }
        }
    }

    private void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
    }
}
