using UnityEngine;

public class ButtonInteraction : MonoBehaviour
{
    public GameObject prisonObject; // ���� ������Ʈ
    public int doorOpen = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // ���� ������Ʈ ����
            Destroy(prisonObject);
            Destroy(gameObject);
            doorOpen = 1;
        }
    }
}
