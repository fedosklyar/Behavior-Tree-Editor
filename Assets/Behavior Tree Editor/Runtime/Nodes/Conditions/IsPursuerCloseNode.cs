using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Is Pursuer Close?", "Condition/Is Pursuer Close?")]
public class IsPursuerCloseNode : ConditionNode
{
    public string pursuerKey;
    public float fleeDistance = 5f;
    protected override bool CheckCondition()
    {
        var pursuer = blackboard[pursuerKey] as Transform;

        if (!pursuer)
        {
            return false;
        }

        return Vector3.Distance(agent.transform.position, pursuer.position)
            < fleeDistance;
    }
}
