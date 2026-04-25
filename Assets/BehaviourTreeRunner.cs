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
    }

    void Update()
    {
        tree.Update();
    }
}
