using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform target; // 타겟 = 플레이어

    public float moveSpeed = 5f; // 적의 이동 속도
    public float rotationSpeed = 5f; // 플레이어를 바라볼 때 회전 속도
    public float attackRange = 2f; // 적의 공격 범위
    public float attackCooldown = 2f; // 공격 쿨다운
    public float knockbackForce = 10f; // 뒤로 밀려나는 힘
    public float maxDistance = 8f; // 플레이어와의 최대 허용 거리

    private Animator animator; // 애니메이터 컴포넌트
    private bool canAttack = true; // 공격 가능 여부
    private bool isAttacking = false; // 공격 중 여부
    private Rigidbody rb; // Rigidbody 컴포넌트

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
                        animator.SetFloat(MoveSpeedHash, direction.magnitude); // MoveSpeed 파라미터 설정
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
