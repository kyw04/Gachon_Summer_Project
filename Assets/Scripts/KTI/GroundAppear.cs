using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundAppear : MonoBehaviour
{
    // ������ ����� Plane�� �ڽ��� RespawnRange ������Ʈ
    public GameObject rangeObject;
    BoxCollider rangeCollider;
    Vector3 Spawnpos;
    Vector3[] SpawnposSave = new Vector3[3];

    [Header("������ �׶��� ����")]
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


        range_X = Random.Range(-19, 34);
        range_Z = Random.Range(-19, 34);
        Vector3 RandomPostion = new Vector3(range_X, 0.08f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }

    public GameObject enemy;
    public GameObject Warn;
    private void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
        
    }

    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            for (int i = 0; i < FireballCount; i++)
            {
                Spawnpos = Return_RandomPosition();
                Instantiate(Warn, Spawnpos, Quaternion.identity);
                StartCoroutine(Spawn_Cor(Spawnpos));
                //Invoke("Summon", 0.5f);
                //Instantiate(enemy, Return_RandomPosition(), Quaternion.identity);
            }
        }
    }

    IEnumerator Spawn_Cor(Vector3 a)
    {
        while (true)
        {
                yield return new WaitForSeconds(1f);
                Instantiate(enemy, a, Quaternion.identity);
                break;
        }
    }


}
