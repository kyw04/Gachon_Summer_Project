using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Stage2_Boss : MonoBehaviour
{
    [SerializeField] int Boss_AttackNum;

    public GameObject meteorPrefab;
    Transform playerTransform;
    IEnumerator attack;
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        attack = Attack_C();
        StartCoroutine(attack);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            StopCoroutine(attack);
    }
    public void Attack()
    {

    }
    IEnumerator Attack_C()
    {
        while (true)
        {
            switch (Boss_AttackNum)
            {
                // �� �ѹ� �ֵθ��� 
                case 1:

                    break;
                // �� �ι� �ֵθ��� 
                case 2:

                    break;
                // ���� ����(���׿� ��ȯ)
                case 3:
                    // �÷��̾��� ��ġ�� ����
                    Vector3 playerPosition = playerTransform.position;
                    // ���׿��� �÷��̾��� ���� ��ġ�� ����
                    Vector3 meteorPosition = new Vector3(playerPosition.x, playerPosition.y + 3f, playerPosition.z);
                    GameObject meteor = Instantiate(meteorPrefab, playerPosition, Quaternion.identity);
                    // ������ ���׿��� ���� �߰� ������ �ʿ��ϴٸ� ���⿡ �߰� �ڵ� �ۼ�
                    break;
            }
            yield return new WaitForSeconds(2f);
        }
    }
}