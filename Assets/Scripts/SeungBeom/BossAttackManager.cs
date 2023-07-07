using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{
    public float WaitSeconds = 10;
    public int Rand;

    public Atk2Pool attack2Pool;      //2번 패턴 풀
    public Atk3Pool a3pool;             //3번 패턴 풀


    public GameObject player;

    GameObject Atk1;
    public bool Atk1Active;
    public GameObject Atk2; // 
    public GameObject Atk3; // 
    // Start is called before the first frame update
    private void Awake()
    {
        Atk1 = transform.GetChild(4).gameObject;
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        Atk1.SetActive(false);
        StartCoroutine(AttackSelect());
        StartCoroutine(ATK1());
    }

    // Update is called once per frame
    void Update()
    {
        if(Atk1Active)
        {
            Atk1.SetActive(true);
        }
        if(!Atk1Active)
        {
            Atk1.SetActive(false);
        }

        if (Rand == 0)
        {
            StartCoroutine(ATK2());
        }
        if (Rand == 1)
        {
            GameObject Atk3 = a3pool.GetItem();

            Atk3.transform.position = new Vector3(player.transform.position.x, -0.1f, player.transform.position.z);
            Atk3.transform.rotation = Quaternion.identity;

            WaitSeconds -= 2;
        }



        if (Input.GetKeyDown(KeyCode.K))
        {   
            GameObject Atk3 = a3pool.GetItem();

            Atk3.transform.position = new Vector3(player.transform.position.x, -0.1f, player.transform.position.z);
            Atk3.transform.rotation = Quaternion.identity;
        }
    }
    IEnumerator AttackSelect()
    {
        while(true)
        {
            yield return new WaitForSeconds(WaitSeconds);
            Rand = Random.Range(0, 2); //  0 운석 / 1 폭발 
            yield return new WaitForSeconds(0.01f);
            Rand = 5;
        }
        
    }
    IEnumerator ATK1()
    {
        while(true)
        {
            Atk1Active = true;
            yield return new WaitForSeconds(5);
            Atk1Active = false;
            yield return new WaitForSeconds(15);
        }
    }
    IEnumerator ATK2()
    {

        GameObject newObj = attack2Pool.GetItem();
        newObj.transform.position = new Vector3(player.transform.position.x, -0.1f, player.transform.position.z);
        newObj.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1.5f);

        newObj = attack2Pool.GetItem();
        newObj.transform.position = new Vector3(player.transform.position.x, -0.1f, player.transform.position.z);
        newObj.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(1.5f);

        newObj = attack2Pool.GetItem();
        newObj.transform.position = new Vector3(player.transform.position.x, -0.1f, player.transform.position.z);
        newObj.transform.rotation = Quaternion.identity;

        WaitSeconds += 2;
        
    }
    


}
