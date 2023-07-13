using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Nav : MonoBehaviour
{
    GameObject pl;

    public Transform player;

    NavMeshAgent nav;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        pl = GameObject.Find("Player");
    }

    private void Update()
    {
        nav.SetDestination(player.position);
    }
}
