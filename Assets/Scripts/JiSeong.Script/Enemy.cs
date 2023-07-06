using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform target; // 플레이어의 Transform 컴포넌트

    public float moveSpeed = 5f; // 적의 이동 속도
    public float rotationSpeed = 5f; // 회전 속도
    public float attackRange = 2f; // 공격 범위
    public float attackDamage = 10f; // 공격력
    public float attackCooldown = 2f; // 공격 쿨다운

    private Animator animator; // Animator 컴포넌트
    private bool canAttack = true; // 공격 가능 여부
    private bool isAttacking = false; // 공격 중 여부

    private void Start()
    {
        animator = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
    }

    private void Update()
    {
        if (target != null)
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
                print("공격");
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

                }
            }
        }
    }

    private void Attack()
    {
        // 플레이어 방향으로 회전
        Vector3 direction = target.position - transform.position;
        direction.y = 0; // 수평 방향으로 제한 (y축 회전 방지)
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // 애니메이션 재생
        animator.SetTrigger("isAttack"); // isAttack 트리거 설정

        // 공격 쿨다운 시작
        canAttack = false;
        isAttacking = true;
        Invoke("ResetAttack", attackCooldown);
    }

    private void ResetAttack()
    {
        canAttack = true;
        isAttacking = false;
        print("초기화");
    }
}
