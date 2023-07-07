using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public Transform player;
    public float speed = 5f;
    public float range = 1f;

    void Update()
    {
        float distance = Vector3.Distance(transform.position, player.position);
        if (distance <= range)
        {
            transform.LookAt(player);

            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        }
    }
}