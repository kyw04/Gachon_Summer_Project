using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UniRx.Triggers;
using Quaternion = UnityEngine.Quaternion;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class PaladinComponent : BattleableComponentBase
{
    public bool isActing;
    public bool isActionable;
    public float coolDown;
    public sbyte phase = 1;
    private Subject<sbyte> _phaseObserver = new Subject<sbyte>();

    //������
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
        ChargeMagic();
    }
    
    void Start()
    {   
        Status = new BattleableVOBase()
        {
            name = Status.name,
            attackPoint = Status.attackPoint,
            maxHealthPoint = Status.maxHealthPoint,
            spd = Status.spd
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
        if (playerInstance is null)
            return;
            
        _distance = Vector3.Distance(this.transform.position, playerInstance.transform.position);
        Think();
        Move();
    }

    private void ChargeMagic()
    {
        var magicPrefab = Resources.LoadAsync<GameObject>("Magic/Metor/Object").asset as GameObject;
        magicInstances = new Queue<MagicComponent>();

        for (var i = 0 ; i < 8; i++)
        {
            var item = Instantiate(magicPrefab).gameObject.GetComponent<MagicComponent>();
            item.gameObject.SetActive(false);
            magicInstances.Enqueue(item);
        }
    }
    
    private void Think()
    {
        if (isActing || !isActionable || healthPoint == 0) return;
        
        isActionable = false;

        Act action = () => { isActing = true; };

        if (phase == 1 && (float)healthPoint / this.Status.maxHealthPoint <= 0.5f)
            action += () =>
            {
                isActionable = false;
                phase = 2;
                healthPointSlider.value = healthPoint;
                animator.SetTrigger("Rage");
                FormChange("Paladin/Phase2");
            };
        
        else if (_distance <= 1.5f)
            action += () =>
            {
                animator.SetTrigger("Kick");
                
                CallMethodWaitForSeconds(1000,() => { isActionable = true; });
            };
        else if (_distance <= 4f)
        {
            action += Attack;

            action += phase == 1 ? () => { } : () =>
            {
                StartCoroutine(UpgradeedAttack());
            };

            action += () => { CallMethodWaitForSeconds(5000, () => { isActionable = true; }); };
        }
        else
            action += () =>
            {
                animator.SetTrigger("Cast");
                StartCoroutine(Casting());
                CallMethodWaitForSeconds((int)coolDown * 1000,() => { isActionable = true; });
            };



        action();
    }

    public void FormChange(string path)
    {
        if (path.Trim() == "") path = "Paladin/Phase2";
        var mat = Resources.LoadAsync(path.Trim()).asset as Material;
        FormChange(mat);
    }

    private void FormChange(Material mat)
    {
        var renderCollection = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var meshRenderer in renderCollection)
        {
            meshRenderer.sharedMaterial = mat;
        }
    }

    public override void Attack()
    {
        isDamageable = false;
        base.Attack();
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
        //1.5초 동안 데미지 입힐 수 없는 상태가 됨.
        CallMethodWaitForSeconds(1500,() => { isDamageable = true; });
        var result = base.ModifyHealthPoint(amount);   
        healthPointSlider.value = healthPoint;
        return result;
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
                isDamageable = true;
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
                CallMethodWaitForSeconds(1000,() => { isActionable = true; });
                break;
            case "RageEnd":
                isActionable = true;
                break;
        }
        StartCoroutine(LookTo(playerInstance.transform));
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
    private IEnumerator LookTo(Transform target)
    {
        isActing = true;
        for (float f = 0; f <= 1; f += 0.04f )
        {
            Vector3 dir = target.position - this.transform.position;

            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, Quaternion.LookRotation(dir), f);
            yield return new WaitForSeconds(0.02f);
        }

        isActing = false;
        transform.LookAt(target.position);
        yield break;
    }

    private IEnumerator UpgradeedAttack()
    {
        while (isAttacking)
        {
            Rigidbody.velocity += lookFoward.normalized * 0.1f;
            yield return new WaitForSeconds(0.01f);
        }
    }
}
