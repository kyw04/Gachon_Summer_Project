// 전투 가능한 오브젝트들이 가져야할 맴버들을 명시하는 추상 클래스
// 적의 행동을 구현할 때 이 클래스를 상속해서 구현해 주세요

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]

public abstract class BattleableComponentBase : MonoBehaviour, IBattleable
{
    public BattleableVOBase Status;

    //현재 체력
    [SerializeField]
    protected int healthPoint;
    protected int StaminaPoint;
    protected float AttackDelay;
    
    protected BattleableDataControllerBase dataController;

    public Animator animator;
    protected Rigidbody Rigidbody;
    //현재 카메라가 바라보고 있는 방향을 바라보고 있는 변수
    public Vector3 lookFoward;
    public Vector3 lookRight;
    
    // BattleableComponentBase.BattleableVOBase.AttackPoint 등으로 공격력, 체력 등의 스테이터스 정보를 가져 올 수 있음
    // ex) : PlayerComponent.Status.AttackPoint 
    // 상속 받은 후 Start문에서 객체 생성해줘야함.
    
    //공격을 시작했을 경우 true로 변했다가 공격 애니메이션이 끝나면 false로
    [SerializeField]
    protected bool isAttacking = false;

    // 전투 가능한 오브젝트들의 스테이터스를 보관할 VO.
    
    private void Awake()
    {
        //컴포넌트를 가져옴
        animator = GetComponent<Animator>();
        Rigidbody = GetComponent<Rigidbody>();
    }

    public virtual void Attack()
    {
        if (isAttacking) return;
        else isAttacking = !isAttacking;
        //공격 구현 
        animator.SetTrigger("Attack");

    }

    public virtual void ModifyHealthPoint(int amount)
    {
        if (amount < 0)
        {
            if ((healthPoint -= amount) < 0)
            {
                healthPoint = 0;
                Die();
                return;
            }
            //데미지 받았을때의 행동 구현
        }
        else
        {
            //체력이 회복 되었을 경우 일어날 동작을 구현
        }

    }
    public virtual void Die()
    {
        //죽었을 경우 기본적으로 실행할 코드
    }
    public abstract void Move();
    protected abstract void OnCollisionEnter(Collision other);
    protected abstract void OnCollisionStay(Collision other);

    #region Animation Event를 위한 메소드

    public abstract void AnimEvt(string cmd);

    #endregion
}