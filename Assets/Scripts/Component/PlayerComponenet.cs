//

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class PlayerComponenet : BattleableComponentBase, IControllable
{
    #region  Variable
    
        public bool isDodging = false;
        public bool isWiring = false;
        public bool isJumping = false;

    #endregion
    
    delegate void Act();

    //Awake는 base에 사용 중이므로 기본 설정은 Start를 통해 해주세요
    private void Start()
    {
        //현재 체력을 최대 체력에 맞춤
        Status = new PlayerVO();
        healthPoint = Status.maxHealthPoint;
    }

    private void FixedUpdate()
    {
        Command();
    }

    #region 기능적인 메소드

    public override void ModifyHealthPoint(int amount)
    {
        if (amount < 0)
        {
            if ((healthPoint -= amount) < 0)
            {
                healthPoint = 0;
                Die();
            }
        }
        else
        {
            //체력이 회복 되었을 경우 일어날 동작을 구현
        }

    } 
    public override void Die()
    {
        base.Die();
        //플레이어의 사망시 발생할 상황을 구현
    }

    #endregion

    #region 플레이어 조작과 관련한 메소드
    public void Command()
    {
        //기본적으로 MOVE함수를 실행시키며 특정한 INPUT이 있으면 그에 맞는 메소드를 실행
        Act action = Move;
        
        //조작키들은 임의로 정해진 키로 작동하므로 나중에 정해지면 수정 바람 (2023. 06. 29)
        if (Input.GetButton("Attack"))
        {
            action = Attack;
        }
        else if (Input.GetButton("Dodge"))
        {
            action = () =>
            {
                if (isDodging) return;
                else isDodging = !isDodging;
                //회피 기능 구현

            };
        }
        else if (Input.GetButton("Wiring"))
        {
            action = () =>
            {
                if (isWiring) return;
                else isWiring = !isWiring;
                //갈고리 이동 기능 구현

            };
        }
        else if (Input.GetButton("Jump"))
        {
            action = () =>
            {

                if (isJumping) return;
                else isJumping = !isJumping;
                //점프 구현
            };
        }

        action();
    }
    public override void Move()
    {
        var direction =
            (Input.GetAxisRaw("Horizontal") * Vector3.right + Input.GetAxisRaw("Vertical") * Vector3.forward) *
            Time.deltaTime;
        Rigidbody.velocity = new Vector3(
            direction.x * Status.spd,
            Rigidbody.velocity.y,
            direction.z * Status.spd);
        transform.LookAt(
            transform.position + direction);
    }
    #endregion
    
    #region Collision Func

    protected override void OnCollisionEnter(Collision other)
    {
        
    }

    protected override void OnCollisionStay(Collision other)
    {
        
    }
    #endregion

}