using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree tree;

    void Start()
    {
        tree = tree.Clone();
        tree.Bind(GetComponent<AiAgent>()); //The Both the Runner and the AiAgent should be attached to the GameObject 
    }

    void Update()
    {
        tree.Update();
    }
}
