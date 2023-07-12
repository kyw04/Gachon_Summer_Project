using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PaladinComponent : BattleableComponentBase
{
    public bool isActing;
    public bool isActionable;
    public bool isDamageable = true;
    public float coolDown;

    //ÇÁ¸®ÆÕ
    public Queue<MagicComponent> magicInstances;

    [SerializeField]
    private PlayerComponent playerInstance;
    public Slider healthPointSlider;
    public Text nameTextField;
    private float _distance = 0;
    delegate void Act();
    // Start is called before the first frame update
    
    public void Awake()
    {
        base.Awake();
        var magicPrefab = Resources.Load("Magic/Metor/Object") as GameObject;
        magicInstances = new Queue<MagicComponent>();

        for (var i = 0 ; i < 8; i++)
        {
            var item = Instantiate(magicPrefab, this.transform, true).gameObject.GetComponent<MagicComponent>();
            item.gameObject.SetActive(false);
            magicInstances.Enqueue(item);
        }
    }
    
    void Start()
    {
        Status = new BattleableVOBase()
        {
            name = Status.name,
            attackPoint = Status.attackPoint,
            maxHealthPoint = Status.maxHealthPoint,
            spd = 1.5f
        };
        playerInstance = GameObject.FindWithTag("Player").GetComponent<PlayerComponent>();
        healthPoint = Status.maxHealthPoint;
        
        healthPointSlider.maxValue = Status.maxHealthPoint;
        healthPointSlider.value = healthPoint;
        nameTextField.text = Status.name;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _distance = Vector3.Distance(this.transform.position, playerInstance.transform.position);
        Think();
        Move();
    }

    private void Think()
    {
        if (isActing || !isActionable || healthPoint == 0) return;

        isActionable = false;
        Act action = () =>
        {
            isActing = true;
            animator.SetTrigger("Cast");
            
            StartCoroutine(Casting());
        };

        if (_distance <= 1.8f)
            action = () =>
            {
                isActing = true;
                animator.SetTrigger("Kick");
            };
        else if (_distance <= 3.2f)
            action = Attack;
        
        StartCoroutine(CallMethodWaitForSeconds(coolDown, () => { isActionable = true;}));
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
        if (isActing || this.healthPoint <= 0) return;
        this.transform.LookAt(playerInstance.transform);

        if (_distance <= 3f)
        {
            Rigidbody.velocity = Vector3.zero;
            animator.SetBool("isWalkingForward", false);
            animator.SetBool("isIdle", true);
        }
        else
        {
            lookFoward = this.transform.forward;
            lookRight = this.transform.right;

            Rigidbody.velocity = lookFoward.normalized * Status.spd;
            animator.SetBool("isWalkingForward", true);
            animator.SetBool("isIdle", false);
        }
    }

    public override int ModifyHealthPoint(int amount)
    {
        if (this.healthPoint == 0) return -1;
        if (!isDamageable) return 0;
        isDamageable = false;
        isActionable = false;
        StopAllCoroutines();
        StartCoroutine(CallMethodWaitForSeconds(2f, () => { isDamageable = true; }));   
        healthPointSlider.value = healthPoint;
        return base.ModifyHealthPoint(amount);
    }

    public override void Die()
    {
        base.Die();
        isActionable = false;
        isActing = true;
        Debug.Log(true);
    }

    protected override void OnCollisionEnter(Collision other)
    {
    }

    protected override void OnCollisionStay(Collision other)
    {
    }

    public override void AnimEvt(string cmd)
    {
        isActing = false;
        switch (cmd)
        {
            case "AttackEnd":
                isAttacking = false;
                //this.transform.LookAt(playerInstance.transform);
                break;
            case "Damaged":
                isAttacking = false;
                isActionable = true;
                break;
            case "Kick":
                playerInstance.ModifyHealthPoint(Status.attackPoint * -1);
                StartCoroutine(playerInstance.HitBack(playerInstance.lookFoward * -1, 0.1f, 3f));
                break;
            case "KickEnd":
                isActionable = true;
                break;
        }
    }

    private IEnumerator Casting()
    {
        for (int i = 0; i <= magicInstances.Count; i++)
        {
            var magicInstance = magicInstances.Dequeue();
            magicInstance.transform.position = Vector3.zero + playerInstance.transform.position +
                new Vector3()
                {
                    x = Random.Range(-10f, 10f),
                    y = 10f,
                    z = Random.Range(-10f, 10f)
                };
            
            magicInstance.gameObject.SetActive(true);
            magicInstance.caster = this;
            magicInstances.Enqueue(magicInstance);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public IEnumerator CallMethodWaitForSeconds(float duration, Action act)
    {
        yield return new WaitForSeconds(duration);
        act();
    }
}
