using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[NodeInfo("Can See Target?", "Condition/Can See Target?")]
public class CanSeeTargetNode : ConditionNode
{
    public string targetKey;
    public float viewDistance = 10f;
    public LayerMask obstacleMask;
    protected override bool CheckCondition()
    {
        var target = blackboard[targetKey] as Transform;

        if (!target)
        {
            return false;
        }

        Vector3 direction = target.position - agent.transform.position;

        if (direction.magnitude > viewDistance)
        {
            return false;
        }

        //Raycast to check whether there is something to block the line of sight
        if (Physics.Raycast(agent.transform.position, direction.normalized, out RaycastHit hit, viewDistance, obstacleMask))
        {
            return false; //smth is on our way
        }

        return true;
    }
}
