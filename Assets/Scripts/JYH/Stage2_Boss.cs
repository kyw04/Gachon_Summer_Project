using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2_Boss : MonoBehaviour
{
    [SerializeField] int Boss_AttackNum;

    public ObjectPoolComponent boss_Attack;
    Transform playerTransform;
    IEnumerator attack;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        attack = Attack_C();
        StartCoroutine(attack);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StopCoroutine(attack);
    }
    public void Attack()
    {

    }
    IEnumerator Attack_C()
    {
        while (true)
        {
            switch (Boss_AttackNum)
            {
                // 낫 한번 휘두르기 
                case 1:

                    break;
                // 낫 두번 휘두르기 
                case 2:

                    break;
                // 마법 공격(메테오 소환)
                case 3:
                    // 플레이어의 위치를 얻어옴
                    Vector3 playerPosition = playerTransform.position;
                    // 메테오를 플레이어의 현재 위치에 생성
                    Vector3 meteorPosition = new Vector3(playerPosition.x, playerPosition.y + 3f, playerPosition.z);
                    GameObject meteor = boss_Attack.GetItem(meteorPosition);
                    boss_Attack.FreeItem(meteor);
                    //// GameObject meteor = Instantiate(meteorPrefab, meteorPosition, Quaternion.identity);
                    //if (meteor.transform.position.y <= -0.5)
                    //    Destroy(meteor);

                    // 생성된 메테오에 대한 추가 설정이 필요하다면 여기에 추가 코드 작성
                    break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}
