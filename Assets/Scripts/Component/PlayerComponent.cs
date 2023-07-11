//

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

public sealed class PlayerComponent : BattleableComponentBase, IControllable
{
    #region  Variable

        public bool isDodging = false;
        public bool isWiring = false;
        public bool isJumping = false;
        public bool isControllable = true;
        public bool isJumpable = true;

        private int AttackStatus = 0;

        #endregion
    delegate void Act();

    //Awake는 base에 사용 중이므로 기본 설정은 Start를 통해 해주세요
    private void Start()
    {
        SetUpPlayer();
    }

    private void FixedUpdate()
    {
        Command();
        Status.position = this.transform.position;
        dataController.UseUpdate(Status.id, Status.position);
        isJumpable = Rigidbody.velocity.y > 0;
    }

    #region 기능적인 메소드

    public override int ModifyHealthPoint(int amount)
    {
        isControllable = false;
        StopAllCoroutines();
        return base.ModifyHealthPoint(amount);
    }

    public override void Die()
    {
        base.Die();
        //플레이어의 사망시 발생할 상황을 구현
        isControllable = false;
    }

    private void SetUpPlayer()
    {
        dataController = new PlayerDataController("/Battleable.db");
        Status = dataController.getDatum();

        animator.enabled = false;

        var playerModel = Resources.Load<GameObject>($"PlayerModels/{Status.modelName}/Model");

        GameObject modelInstance = null;

        try
        {
            modelInstance = Instantiate(playerModel);
        }
        catch
        {
            Debug.Log($"Do Not Found {Status.modelName} Prefab.\n Use Dummy Object");
            playerModel = Resources.Load<GameObject>("PlayerModels/Dummy/Model");
            modelInstance = Instantiate(playerModel);
        }
        finally
        {

            foreach (var child in this.transform.GetComponentsInChildren<Transform>())
            {
                if (child.transform == this.transform || child.gameObject.name == this.gameObject.name)
                    continue;
                child.gameObject.SetActive(false);
            }

            animator.avatar = modelInstance.GetComponent<Animator>().avatar;

            modelInstance.transform.position = Vector3.zero;
            modelInstance.transform.parent = this.transform;
        }
        
        GC.SuppressFinalize(playerModel);
        
        this.transform.position = Status.position;
        healthPoint = Status.maxHealthPoint;
        StaminaPoint = Status.maxStaminaPoint;

        animator.Rebind();
        animator.enabled = true;

        Destroy(this.transform.GetChild(0).gameObject);
    }

    #endregion

    #region 플레이어 조작과 관련한 메소드

    public override void Attack()
    {
        base.Attack();
        animator.SetInteger("AttackType", AttackStatus++ % 2 );
    }

    public void Command()
    {
        //기본적으로 MOVE함수를 실행시키며 특정한 INPUT이 있으면 그에 맞는 메소드를 실행
        Act action = Move;
        
        //조작키들은 임의로 정해진 키로 작동하므로 나중에 정해지면 수정 바람 (2023. 06. 29)
        if (Input.GetButton("Attack"))
        {
            if (isControllable) action = Attack;
            isControllable = false;
        }
        else if (Input.GetButton("Dodge"))
        {
            action = () =>
            {
                if (isDodging || !isControllable) return;
                else isDodging = !isDodging;
                isControllable = false;

                //this.transform.forward = lookFoward * (Input.GetAxisRaw("Vertical") == -1 ? -1 : 1);
                animator.SetTrigger("Rolling");
                StartCoroutine(Roll(Input.GetAxisRaw("Vertical")));


            };
        }
        else if (Input.GetButton("Wiring"))
        {
            action = () =>
            {
                if (isWiring || !isControllable) return;
                else isWiring = !isWiring;
                isControllable = false;
                //갈고리 이동 기능 구현

            };
        }
        else if (Input.GetButton("Jump"))
        {
            action = () =>
            {
                if (healthPoint <= 0) return;
                else isJumping = !isJumping;
                isControllable = false;
                //점프 구현
                animator.SetTrigger("Jump");
            };
        }

        action();
    }
    public override void Move()
    {
        if (!isControllable) return;
        var dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        
        //에니메이터에 있는 bool 타입의 파라미터들을 한 번에 false로
        var boolParamName =
            from param in animator.parameters
            where param.type == AnimatorControllerParameterType.Bool
            select param.name;
        foreach (var paramName in  boolParamName)
            animator.SetBool(paramName, false);
        
        
        //플레이어가 이동 중 일 경우
        if (dir.magnitude != 0)
        {
            this.transform.forward = lookFoward;
            var moveDir = lookFoward * dir.y + lookRight * dir.x;
            if (dir.x == 0)
            {
                animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
            }
            else
            {
                if (dir.y == 0)
                {
                    
                    animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                }
                else
                {
                    //오른쪽 앞 & 뒤
                    animator.SetBool(dir.y > 0 ? "isWalkingForward" : "isWalkingBackward", true);
                    animator.SetBool(dir.x < 0 ? "isWalkingLeft" : "isWalkingRight", true);
                    moveDir /= Mathf.Sqrt(2);
                }
            }

            transform.position += moveDir * Time.deltaTime * Status.spd;
        }
        else
        {
            animator.SetBool("isIdle", true);
        }
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
    
    public override void AnimEvt(string cmd)
    {
        isControllable = true;
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                break;
            case "RollEnd":
                isDodging = false;
                break;
            case "Jump":
                StartCoroutine(Jump());
                break;
            case "JumpEnd":
                isJumping = false;
                break;            
            case "Damaged":
                isAttacking = false;
                isJumping = false;
                isDodging = false;
                break;
        }
    }

    #region IEnumerator

    IEnumerator Roll(float status)
    {
        var dir = lookFoward;
        var coefficient = (Mathf.Abs(status) > 0.5f ? status : 1);
        this.transform.forward = dir * coefficient;
        while (isDodging)
        {
            this.transform.position += dir.normalized * 0.1f * coefficient;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }
    IEnumerator Jump()
    {
        while (isJumping)
        {
            this.transform.position += Vector3.up * 0.15f;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    public IEnumerator HitBack(Vector3 dir, float duration, float power)
    {
        var time = 0f;
        while (true)
        {
            this.transform.position += dir.normalized * 0.05f * power;
            time += 0.02f;
            if (time >= duration) yield break;
            yield return new WaitForSeconds(0.01f);
        }
        yield break;
    }

    #endregion

}