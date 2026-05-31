using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[NodeInfo("Inverter", "Decorator/Inverter")]
public class InverterNode : DecoratorNode
{
    protected override void OnStart() { }
    protected override void OnStop() { }

    protected override State OnUpdate()
    {
        return child.Update() switch
        {
            State.Success => State.Failure,
            State.Failure => State.Success,
            _ => State.Running
        };
    }

    
}
