using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[NodeInfo("Parallel", "Composite/Parallel")]
public class ParallelNode : CompositeNode
{
    public int successThreshold = 1;
    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        int childrenCount = this.children.Count;

        int successCount = 0;
        int failureCount = 0;

        for (int i = 0; i < childrenCount; i++)
        {
            var childState = children[i].Update();
            if (childState == State.Success)
                ++successCount;
            else if (childState == State.Failure)
                ++failureCount;
        }

        if (successCount >= successThreshold)
            return State.Success;

        if (failureCount > (childrenCount - successThreshold))
            return State.Failure;

        return State.Running;
    }
}
