using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//[System.Serializable]
public abstract class ConditionNode : Node
{
    public bool invert = false;

    protected override void OnStart()
    {

    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        bool isTrue = CheckCondition();

        if (invert)
        {
            isTrue = !isTrue;
        }

        if (isTrue)
        {
            return State.Success;
        }
        return State.Failure;
    }

    protected abstract bool CheckCondition();
}
