using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField] GameObject m_goPrefab = null;
    public GameObject[] zombieObjects; // Zombie 태그를 가진 오브젝트 배열
    public GameObject[] BossObjects;
    private bool BarOn = false;
    List<Transform> m_objectList = new List<Transform>();
    List<GameObject> m_hpBarList = new List<GameObject>();

    Camera m_cam = null;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        zombieObjects = GameObject.FindGameObjectsWithTag("Zombie");


        if (zombieObjects.Length == 0)
        {
            BossObjects = GameObject.FindGameObjectsWithTag("Boss");

            if (BossObjects.Length >= 1)
            {
                HpBarOn();
            }

            for (int i = 0; i < m_objectList.Count; i++)
            {
                m_hpBarList[i].transform.position = m_cam.WorldToScreenPoint(m_objectList[i].position + new Vector3(0, 3f, 0));
            }
        }
    }
    void HpBarOn()
    {
        if (BarOn == false)
        {
            BarOn = true;
            m_cam = Camera.main;

            GameObject[] t_objects = GameObject.FindGameObjectsWithTag("Boss");
            for (int i = 0; i < t_objects.Length; i++)
            {
                m_objectList.Add(t_objects[i].transform);
                GameObject t_hpbar = Instantiate(m_goPrefab, t_objects[i].transform.position, Quaternion.identity, transform);
                m_hpBarList.Add(t_hpbar);
            }
        }
    }
}
