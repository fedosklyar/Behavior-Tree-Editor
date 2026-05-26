using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveForwardNode : ActionNode
{
    public float speed;
    private Transform objectsTransform;
    protected override void OnStart()
    {
        objectsTransform = this.agent.gameObject.transform;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(!objectsTransform)
            return State.Failure;
            
        objectsTransform.position += objectsTransform.forward * speed * Time.deltaTime;
        return State.Running;
    }
}
