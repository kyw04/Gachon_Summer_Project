using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PaladinComponent : BattleableComponentBase
{
    [SerializeField]
    private PlayerComponent playerInstance;
    // Start is called before the first frame update
    void Start()
    {
        playerInstance = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Think();
    }

    private void Think()
    {
        if (Vector3.Distance(this.transform.position, playerInstance.transform.position) <= 2f)
        {
            Attack();
        }
        else
        {
            Move();
        }
    }

    public override void Attack()
    {
        base.Attack();
        animator.SetBool("isWalkingForward", false);
    }

    public override void Move()
    {
        this.transform.LookAt(playerInstance.transform);

        lookFoward = this.transform.forward;
        lookRight = this.transform.right;

        Rigidbody.velocity = (lookRight + lookFoward).normalized * Status.spd;
        
        animator.SetBool("isWalkingForward", true);
    }

    protected override void OnCollisionEnter(Collision other)
    {
    }

    protected override void OnCollisionStay(Collision other)
    {
    }

    public override void AnimEvt(string cmd)
    {
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                break;
        }
    }
}
