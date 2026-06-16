using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

[NodeInfo("Can See Target?", "Condition/Can See Target?")]
public class CanSeeTargetNode : ConditionNode
{
    public string targetKey;
    public float viewDistance = 10f;
    public float loseDistance = 12f; // only lose sight at greater distance
    private bool currentlySeeingTarget = false;
    public LayerMask obstacleMask;
    protected override bool CheckCondition()
    {
        var target = blackboard[targetKey] as Transform;
        Debug.Log($"The target is {target.gameObject.name}");
        
        if (!target)
        {
            return false;
        }

        Vector3 direction = target.position - agent.transform.position;
        float distance = direction.magnitude;
        Debug.Log($"The distance value is: {distance}; view distance: {viewDistance}");

        //float threshold = currentlySeeingTarget ? loseDistance : viewDistance;

        if (distance > viewDistance)
        {
            //currentlySeeingTarget = false;
            return false;
        }

        // Cast slightly above ground to avoid self-hit
        Vector3 rayOrigin = agent.transform.position + Vector3.up * 0.5f;

        //Raycast to check whether there is something to block the line of sight
        if (Physics.Raycast(rayOrigin, direction.normalized, out RaycastHit hit, viewDistance, obstacleMask))
        {
            Debug.Log($"Raycast hit: {hit.collider.gameObject.name}");
            //currentlySeeingTarget = false;
            return false; //smth is on our way
        }

        //currentlySeeingTarget = true;
        return true;
    }
}
