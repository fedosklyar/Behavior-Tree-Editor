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


        return isTrue ? State.Success : State.Failure;
    }

    protected abstract bool CheckCondition();
}
