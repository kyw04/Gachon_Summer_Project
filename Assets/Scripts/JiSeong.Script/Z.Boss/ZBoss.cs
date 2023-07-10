using UnityEngine;

public class ZBoss : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform 컴포넌트

    public float moveSpeed = 5f; // 적의 이동 속도
    public float rotationSpeed = 5f; // 회전 속도
    public float attackRange = 2f; // 공격 범위
    public float attackDamage = 10f; // 공격력
    public float attackCooldown = 2f; // 공격 쿨다운
    public float pushForce = 5f; // 충돌로 인한 밀리는 힘
    public int EnemyHp = 3;
    public float ignoreCollisionDuration = 3f; // 충돌 무시 기간

    private Animator animator; // Animator 컴포넌트
    private bool canAttack = true; // 공격 가능 여부
    private bool isAttacking = false; // 공격 중 여부
    private Rigidbody rb; // Rigidbody 컴포넌트
    private float ignoreCollisionTimer; // 충돌 무시 타이머
    private bool isDying = false;
    private Renderer objectRenderer;

    private static readonly int MoveSpeedHash = Animator.StringToHash("MoveSpeed");
    private static readonly int AttackTriggerHash = Animator.StringToHash("Attack");
    private static readonly int HitTriggerHash = Animator.StringToHash("Hit");

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        rb = GetComponent<Rigidbody>(); // Rigidbody 컴포넌트 가져오기
        objectRenderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (target != null)
        {
            if (EnemyHp > 0)
            {
                // 플레이어 방향 구하기
                Vector3 direction = target.position - transform.position;
                direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
                direction.Normalize(); // 벡터를 단위 벡터로 정규화

                // 적과 플레이어 사이의 거리 계산
                float distance = Vector3.Distance(transform.position, target.position);

                if (distance <= attackRange && canAttack && !isAttacking)
                {
                    // 플레이어가 공격 범위 내에 있고 공격 가능하며 공격 중이 아닌 경우
                    Attack();
                }
                else
                {
                    if (distance >= attackRange)
                    {
                        // 적 이동
                        transform.position += direction * moveSpeed * Time.deltaTime;

                        // 적 회전
                        Quaternion targetRotation = Quaternion.LookRotation(direction);
                        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                        // 애니메이션 재생
                        if (animator != null)
                        {
                            animator.SetTrigger("isWalk"); // MoveSpeed 파라미터 설정
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

        // 4초 뒤에 오브젝트 사라지게 하기
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        // 충돌이 발생한 경우
        if (collision.gameObject.CompareTag("Player"))
        {

            ignoreCollisionTimer = ignoreCollisionDuration;
            StartCoroutine(ResetIgnoreCollisionTimer());
        }
    }
    private System.Collections.IEnumerator ResetIgnoreCollisionTimer()
    {
        // HP를 감소시킴
        EnemyHp--;

        if (EnemyHp <= 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
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
            // 플레이어 방향으로 회전
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = targetRotation;

            // 애니메이션 재생
            if (animator != null)
            {
                animator.SetTrigger("isAttack"); // Attack 트리거 설정
            }

            // 플레이어에게 공격력을 가진 데미지를 입힘


            // 공격 쿨다운 시작
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
