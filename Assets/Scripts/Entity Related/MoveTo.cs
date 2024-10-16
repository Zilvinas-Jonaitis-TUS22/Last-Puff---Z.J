using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MoveTo : MonoBehaviour
{
    public Transform target;

    private NavMeshAgent _agent ;
    private ShadowMovement _shadowMovement;

    void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _shadowMovement = GetComponent<ShadowMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        _agent.SetDestination(target.position);
    }
}
