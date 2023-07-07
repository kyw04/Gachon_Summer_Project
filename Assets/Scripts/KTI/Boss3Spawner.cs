using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss3Spawner : MonoBehaviour
{
    public bool enableSpawn = false;

    //Prefab을 받을 public 변수
    public GameObject Enemy;

    // 카메라 크기
    float camera_minx;
    float camera_maxx;
    float camera_miny;
    float camera_maxy;

    // 동시 적 스폰 갯수
    int enemyVisibleLimit;
    // 최대 적 스폰 갯수
    int enemySpawnLimit;


    // 생성된 객체를 받아올 배열
    public GameObject[] enemies;

    void SpawnEnemy()
    {
        //적이 나타날 X좌표를 랜덤으로 생성해 줍니다.
        float randomX = Random.Range(camera_minx, camera_maxx);
        if (enableSpawn)
        {
            // Prefub의 태그를 이용해서 적 갯수 찾아오기
            enemies = GameObject.FindGameObjectsWithTag("Enemy");
            if (enemies.Length < 20)
            {
                //랜덤한 위치와, 화면 제일 위에서 Enemy를 하나 생성해줍니다.
                GameObject enemy = (GameObject)Instantiate(Enemy, new Vector3(randomX, camera_maxy, 0f), Quaternion.identity);
            }

        }
    }
    void Start()
    {
        // 카메라 크기 받아오기
        var height = 2 * Camera.main.orthographicSize;
        var width = height * Camera.main.aspect;
        camera_maxy = -58;
        camera_miny = -camera_maxy;
        camera_maxx = 0.5f * width;
        camera_minx = -58;

        // 적 스폰 관련 값 초기화
        enemyVisibleLimit = 20;
        enemySpawnLimit = 100;


        //3초후 부터, SpawnEnemy함수를 1초마다 반복해서 실행 시킵니다.
        InvokeRepeating("SpawnEnemy", 3, 10);
    }
    void Update()
    {

    }
}
