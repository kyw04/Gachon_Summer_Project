using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject prisonObject; // 감옥 오브젝트
    public int doorOpen = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 감옥 오브젝트 삭제
            Destroy(prisonObject);
            Destroy(gameObject);
            doorOpen = 1;
        }
    }
}
