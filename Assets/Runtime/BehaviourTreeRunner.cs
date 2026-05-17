using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    [Header("Blackboard Overrides")]
    // This allows you to set specific values right here on the GameObject
    public List<BlackboardEntry> localValues;

    void Start()
    {
        tree = tree.Clone();
        tree.Bind(GetComponent<AiAgent>()); //Both the Runner and the AiAgent should be attached to the GameObject 

        //Update blackboard
        tree.blackboard.AddValues(localValues);
        tree.blackboard.PopulateFromEntries();
    }

    void Update()
    {
        tree.Update();
    }
}
