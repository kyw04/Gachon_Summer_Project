using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Spawner : MonoBehaviour
{
    public bool enableSpawn = false;

    //Prefab�� ���� public ����
    public GameObject Enemy;

    // ī�޶� ũ��
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
            if (enemies.Length < 20)
            {
                //������ ��ġ��, ȭ�� ���� ������ Enemy�� �ϳ� �������ݴϴ�.
                GameObject enemy = (GameObject)Instantiate(Enemy, new Vector3(randomX, camera_maxy, 0f), Quaternion.identity);
            }

        }
    }
    void Start()
    {
        // ī�޶� ũ�� �޾ƿ���
        var height = 2 * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
        camera_maxy = -58;
        camera_miny = -camera_maxy;
        camera_maxx = 0.5f * width;
        camera_minx = -58;

        // �� ���� ���� �� �ʱ�ȭ
        enemyVisibleLimit = 20;
        enemySpawnLimit = 100;


        //3���� ����, SpawnEnemy�Լ��� 1�ʸ��� �ݺ��ؼ� ���� ��ŵ�ϴ�.
        InvokeRepeating("SpawnEnemy", 3, 10);
    }
    void Update()
    {

    }
}
