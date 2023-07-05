using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{


    public GameObject player;

    public GameObject Atk2; // ������ ���� �� ���׿�
    public GameObject Atk3; // ������ ���� �� ���� , ����
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.J))
        {
            Instantiate(Atk2, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            Instantiate(Atk3, new Vector3(player.transform.position.x, 0, player.transform.position.z), Quaternion.identity);
        }

    }
}
