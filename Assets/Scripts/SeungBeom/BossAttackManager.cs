using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttackManager : MonoBehaviour
{

    public GameObject player;

    public GameObject Atk2;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        StartCoroutine(Atk());
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    //АјАн
    IEnumerator Atk()
    {
        while(true)
        {
            yield return new WaitForSeconds(3);
            Instantiate(Atk2, player.transform.position, Quaternion.identity);
        }
        

    }
}
