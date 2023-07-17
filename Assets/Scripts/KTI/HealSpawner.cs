using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealSpawner : MonoBehaviour
{
    // ������ ����� Plane�� �ڽ��� RespawnRange ������Ʈ
    public GameObject rangeObject;
    BoxCollider rangeCollider;

    public int FireballCount;

    private void Awake()
    {
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
    }

    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // �ݶ��̴��� ����� �������� bound.size ���
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;



        range_X = Random.Range(-15, 25);
        range_Z = Random.Range(-15, 25);


        Vector3 RandomPostion = new Vector3(range_X, 2.5f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

    public GameObject enemy;
    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            // ���� ��ġ �κп� ������ ���� �Լ� Return_RandomPosition() �Լ� ����
            //GameObject instantCapsul = Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            for (int i = 0; i < FireballCount; i++)
            {
                Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            }
        }
    }
}
