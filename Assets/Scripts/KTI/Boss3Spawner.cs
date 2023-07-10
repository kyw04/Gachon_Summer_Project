using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Spawner : MonoBehaviour
{
    public bool enableSpawn = false;

    //Prefab�� ���� public ����
    public GameObject Enemy;

    float camera_minx;
    float camera_maxx;
    float camera_miny;
    float camera_maxy;
   

    // ���� �� ���� ����
    int enemyVisibleLimit;
    // �ִ� �� ���� ����
    int enemySpawnLimit;


    // ������ ��ü�� �޾ƿ� �迭
    public GameObject[] enemies;

    void SpawnEnemy()
    {
        //���� ��Ÿ�� X��ǥ�� �������� ������ �ݴϴ�.
        float randomX = Random.Range(camera_minx, camera_maxx);
        if (enableSpawn)
        {
            // Prefub�� �±׸� �̿��ؼ� �� ���� ã�ƿ���
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < 2)
            {
                //������ ��ġ��, ȭ�� ���� ������ Enemy�� �ϳ� �������ݴϴ�.
                GameObject enemy = (GameObject)Instantiate(Enemy, new Vector3(randomX, camera_maxy, 0), Quaternion.identity);
            }

        }
    }
    void Start()
    {
        // ī�޶� ũ�� �޾ƿ���
        var height = 2 * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
        camera_maxy = 1;
        camera_miny = 1;
        camera_maxx = -3;
        camera_minx = 4;

        // �� ���� ���� �� �ʱ�ȭ
        enemyVisibleLimit = 100;
        enemySpawnLimit = 2;


        //5���� ����, SpawnEnemy�Լ��� 20�ʸ��� �ݺ��ؼ� ���� ��ŵ�ϴ�.
        InvokeRepeating("SpawnEnemy", 5, 5);
    }
    void Update()
    {

    }
}
