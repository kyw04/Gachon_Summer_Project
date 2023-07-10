using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class PaladinComponent : BattleableComponentBase
{
    public bool isActing;
    
    [SerializeField]
    private PlayerComponent playerInstance;

    delegate void Act();
    // Start is called before the first frame update
    void Start()
    {
        Status = new BattleableVOBase(){spd = 2};
        playerInstance = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Think();
    }

    private void Think()
    {
        if (isActing) return;

        Act action = Move;
        if (Vector3.Distance(this.transform.position, playerInstance.transform.position) <= 3f)
        {
            switch (Random.Range(0,2))
            {
                case 0:
                    action = Attack;
                    break;
                case 1:
                    action = () =>
                    {
                        isActing = true;
                        animator.SetTrigger("Cast");
                    };
                    break;
            }
        }

        action();
    }

    public override void Attack()
    {
        base.Attack();
        isActing = true;
        animator.SetBool("isWalkingForward", false);
    }

    public override void Move()
    {
        if (isActing) return;
        this.transform.LookAt(playerInstance.transform);

        lookFoward = this.transform.forward;
        lookRight = this.transform.right;

        Rigidbody.velocity = lookFoward.normalized * Status.spd;
        
        animator.SetBool("isWalkingForward", true);
        animator.SetBool("isIdle", false);
    }

    protected override void OnCollisionEnter(Collision other)
    {
    }

    protected override void OnCollisionStay(Collision other)
    {
    }

    public override void AnimEvt(string cmd)
    {
        isActing = !isActing;
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                this.transform.LookAt(playerInstance.transform);
                break;
            case "Damaged":
                isAttacking = false;;
                break;
        }
    }
    
    public IEnumerator CallMethodWaitForSeconds(float duration, Action act)
    {
        yield return new WaitForSeconds(duration);
        act();
    }
}
