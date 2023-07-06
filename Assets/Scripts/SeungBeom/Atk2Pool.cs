using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Atk2Pool : MonoBehaviour
{


    public GameObject Prefab = null;
    public int maxCount = 5;

    public Queue<GameObject> queue = new Queue<GameObject>();
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i=0; i < maxCount; i++)  //������Ʈ�� 20�� �������ѳ���.
        {
            GameObject PoolObject = Instantiate(Prefab, transform.parent);
            queue.Enqueue(PoolObject);
            PoolObject.SetActive(false);
        }
    }

    public void Enqueue(GameObject PoolObject)      //�ٽ� ť�� ����ִ� �Լ�
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

        /*
        StartCoroutine(WAIT());
        IEnumerator WAIT()
        {
            yield return new WaitForSeconds(10f);
            queue.Enqueue(PoolObject);
        }
        */
    }

    


}
