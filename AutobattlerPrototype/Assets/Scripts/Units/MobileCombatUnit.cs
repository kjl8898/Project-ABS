using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MobileCombatUnit : CombatUnit
{
    // VARIABLES
    private NavMeshAgent agent;

    private bool hasTargetPos = false; // Nothing would be this low in the map
    [SerializeField] Vector3 targetPosition = Vector3.zero;
    [SerializeField] GameObject testTarget;
    [SerializeField] private float moveSpeed;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();

        agent = GetComponent<NavMeshAgent>();
        moveSpeed = agent.speed;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        UpdateTargetPos();

        if(isFiring)
        {
            agent.speed = 0;
        }
        else
        {
            agent.speed = moveSpeed;
        }
    }

    private void UpdateTargetPos()
    {
        if (targetUnit != null)
        {
            agent.destination = targetUnit.transform.position;
        }
        else
        {
            agent.destination = testTarget.transform.position;
        }
    }
}
