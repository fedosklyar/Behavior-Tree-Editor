using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[NodeInfo("Flee", "Action/Movement/Flee")]
public class FleeNode : ActionNode
{
    public string pursuerKey;
    public float fleeDistance = 10f;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        agent.navMeshAgent.ResetPath();
    }

    protected override State OnUpdate()
    {
        var pursuer = blackboard[pursuerKey] as Transform;
        if (!pursuer)
        {
            return State.Failure;
        }

        Vector3 fleeDirection = (agent.transform.position - pursuer.position).normalized;
        Vector3 fleeTarget = agent.transform.position + fleeDirection * fleeDistance;

        if (NavMesh.SamplePosition(fleeTarget, out NavMeshHit hit, fleeDistance, NavMesh.AllAreas))
        {
            agent.navMeshAgent.SetDestination(hit.position);
        }

        return State.Running;
    }
}
