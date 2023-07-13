using OpenCover.Framework.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyLine : MonoBehaviour
{
    public ObjectPoolComponent pool;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Stage2_Meteor"))
        {
            Vector3 pos = other.transform.position;
            pos.y = other.transform.localScale.y * 0.4f;

            GameObject Destory_partical = pool.GetItem(pos);
            pool.FreeItem(Destory_partical, 2f);
        }
    }
}
