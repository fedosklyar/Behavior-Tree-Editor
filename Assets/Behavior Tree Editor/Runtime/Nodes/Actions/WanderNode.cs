using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using UnityEngine.UI;


[NodeInfo("Wander", "Action/Movement/Wander")]
public class WanderNode : ActionNode
{
    public float wanderRadius = 10f;
    public float reachedThreshold = 1f;
    protected override void OnStart()
    {
        SetNewDestination();
    }
    protected override void OnStop()
    {
        agent.navMeshAgent.ResetPath();
    }

    protected override State OnUpdate()
    {
        if (!agent.navMeshAgent.pathPending && agent.navMeshAgent.remainingDistance < reachedThreshold)
        {
            SetNewDestination();
        }

        return State.Running;
    }
    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += agent.transform.position;

        if (NavMesh.SamplePosition(randomDirection, out NavMeshHit hit, wanderRadius, NavMesh.AllAreas))
        {
            agent.navMeshAgent.SetDestination(hit.position);
        }
    }
}
