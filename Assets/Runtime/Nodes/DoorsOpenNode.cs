using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsOpenNode : ConditionNode
{
    public string objectKey;

    //private GameObject doors;

    protected override bool CheckCondition()
    {
        var doors = this.blackboard[objectKey] as GameObject;

        if (doors)
        {
            return doors.activeSelf;
        }

        return false;
    }

}
