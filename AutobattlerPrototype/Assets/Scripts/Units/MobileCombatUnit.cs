using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class MobileCombatUnit : CombatUnit
{
    // VARIABLES
    private NavMeshAgent agent;

    private bool hasTargetPos = false;
    [SerializeField] Vector3 targetPosition = Vector3.zero;
    [SerializeField] private float moveSpeed;

    [SerializeField] private List<Vector3> patrolLocations = new List<Vector3>();

    public List<Vector3> PatrolLocations
    {
        set { patrolLocations = value; }
    }
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
        if(patrolLocations.Count > 0)
        {
            if (targetPosition == Vector3.zero)
            {
                targetPosition = patrolLocations[Random.Range(0, patrolLocations.Count)];
                agent.destination = targetPosition;
                return;
            }

            if (Vector3.Distance(transform.position, targetPosition) <= 10)
            {
                targetPosition = patrolLocations[Random.Range(0, patrolLocations.Count)];
                agent.destination = targetPosition;
            }
        }
    }
    private void OnDrawGizmosSelected()
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path);

        for(int i = 0; i < path.corners.Length - 1; i++)
        {
            Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }
    }
}
