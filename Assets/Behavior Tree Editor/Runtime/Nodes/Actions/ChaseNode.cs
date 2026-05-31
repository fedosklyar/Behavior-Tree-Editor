using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Chase", "Action/Movement/Chase")]
public class ChaseNode : ActionNode
{
    public string targetKey;


    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {
        agent.navMeshAgent.ResetPath();
    }

    protected override State OnUpdate()
    {
        var target = blackboard[targetKey] as Transform;
        if (!target)
        {
            return State.Failure;
        }

        agent.navMeshAgent.SetDestination(target.position);
        return State.Running;
    }
}
