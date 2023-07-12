using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk1Pool : MonoBehaviour
{
    public GameObject P1Bullet;

    private Queue<GameObject> queue = new Queue<GameObject>();
    public int MaxCount = 125;

    public float FireRate = 0.3f;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < MaxCount; i++)
        {
            GameObject PoolObject = Instantiate(P1Bullet);
            queue.Enqueue(PoolObject);
            P1Bullet.SetActive(false);
            PoolObject.hideFlags = HideFlags.HideInHierarchy;
        }
        
        StartCoroutine(BulletFire());
    }

    public void PutQueue(GameObject PoolObject)      //다시 큐에 집어넣는 함수
    {
        queue.Enqueue(PoolObject);
        PoolObject.SetActive(false);
        
    }

    public GameObject GetItem()
    {
        GameObject PoolObject = queue.Dequeue();
        PoolObject.SetActive(true);

        queue.Enqueue(PoolObject);
        return PoolObject;
    }
    IEnumerator BulletFire()
    {
        while (true)
        {
            yield return new WaitForSeconds(FireRate);
            GetItem();
            Debug.Log("총알 발싸!");
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
