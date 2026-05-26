using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveLeftNode : ActionNode
{
    public float speed;
    public float time;

    private float elapsed;
    private Transform objectsTransform;
    protected override void OnStart()
    {
        objectsTransform = this.agent.gameObject.transform;
        elapsed = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if(!objectsTransform)
            return State.Failure;
        
        elapsed += Time.deltaTime;

        objectsTransform.position += - objectsTransform.right * speed * Time.deltaTime;

        if (elapsed > time)
            return State.Success;

        return State.Running;
    }
}
