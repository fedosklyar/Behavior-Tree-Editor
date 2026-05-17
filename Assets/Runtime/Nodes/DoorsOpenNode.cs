using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsOpenNode : ConditionNode
{
    public string objectKey;

    //private GameObject doors;

    protected override bool CheckCondition()
    {
        // Debug.Log("In check node");
        
        // foreach (var entry in this.blackboard.getDictionary())
        // {
        //     Debug.Log($"The key is: {entry.Key}; and the value is: {entry.Value} ");
        // }

        var doors = this.blackboard[objectKey] as GameObject;

        if (doors)
        {
            return !doors.activeSelf;
        }

        return false;
    }

}
