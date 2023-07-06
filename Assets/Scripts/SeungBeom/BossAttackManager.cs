using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{

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
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Atk1Active = true;
            Debug.Log("atk1 활성화");
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            Atk1Active = false;
            Debug.Log("atk1 비활성화");
        }

        if(Atk1Active)
        {
            Atk1.SetActive(true);
        }
        if(!Atk1Active)
        {
            Atk1.SetActive(false);
        }    



        if (Input.GetKeyDown(KeyCode.J))
        {
            GameObject newObj = attack2Pool.GetItem();
            newObj.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            newObj.transform.rotation = Quaternion.identity;
             //Instantiate(Atk2, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        }



        if (Input.GetKeyDown(KeyCode.K))
        {
            GameObject Atk3 = a3pool.GetItem();

            Atk3.transform.position = new Vector3(player.transform.position.x, 0, player.transform.position.z);
            Atk3.transform.rotation = Quaternion.identity;
        }

    }
}
