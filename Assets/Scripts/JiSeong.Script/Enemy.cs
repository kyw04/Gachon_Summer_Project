using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 5f; // 적의 이동 속도
    public float attackRange = 0.5f; // 공격 범위
    public int damage = 10; // 공격력

    private Transform player; // 플레이어의 위치
    private Animator animator; // 애니메이터 컴포넌트

    private bool isAttacking = false; // 현재 공격 중인지 여부

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform; // 플레이어의 태그가 "Player"인 오브젝트를 찾아서 위치를 가져옴
        animator = GetComponent<Animator>(); // 애니메이터 컴포넌트 가져오기
    }

    private void Update()
    {
        if (player == null)
        {
            return; // 플레이어가 없으면 동작하지 않음
        }

        if (Vector3.Distance(transform.position, player.position) > attackRange)
        {
            // 플레이어를 향해 이동
            transform.position = Vector3.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
        }

        // 플레이어와의 거리가 공격 범위 이내인 경우 공격
        if (Vector3.Distance(transform.position, player.position) <= attackRange)
        {
            AttackPlayer();
            animator.SetTrigger("Attack");
            isAttacking = true;
        }

        Vector3 direction = player.position - transform.position;
        direction.y = 0; // 수평 방향만 고려하고 y축 회전은 제한
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
    }

    private void AttackPlayer()
    {
        
    }
}