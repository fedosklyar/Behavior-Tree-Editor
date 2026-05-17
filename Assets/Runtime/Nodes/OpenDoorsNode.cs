using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class OpenDoorsNode : ActionNode
{
    public string objectKey;
    public float openingTime;

    private GameObject doorsObject;

    private float elapsed;
    protected override void OnStart()
    {
        doorsObject = this.blackboard[objectKey] as GameObject;
        elapsed = 0;
    }

    protected override void OnStop()
    {
        
    }

    protected override State OnUpdate()
    {
        if (!doorsObject)
            return State.Failure;

        if (!doorsObject.activeSelf)
            return State.Success;

        elapsed += Time.deltaTime;

        if (elapsed > openingTime)
        {
            doorsObject.SetActive(false); //Opens the door
            return State.Success;
        }

        return State.Running;
    }
}
