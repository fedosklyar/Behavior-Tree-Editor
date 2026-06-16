using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Suquencer", "Composite/Sequncer")]
public class SequencerNode : CompositeNode
{
    int current;
    protected override void OnStart()
    {
        //current = 0;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        foreach (var child in children)
        {
            switch (child.Update())
            {
                case State.Running:
                    return State.Running;
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    continue; // move to next child in same tick
            }
        }
        
        return State.Success; // all children succeeded
    }
}