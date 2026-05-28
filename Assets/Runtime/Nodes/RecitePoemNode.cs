using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class PoetryPiece
{
    public string piece;
    public float timeForPiece;
}

[NodeInfo("Recite Poem", "Action/Recite Poem")]
public class RecitePoemNode : ActionNode
{

    private float elapsed;
    private int currentPiece;

    public List<PoetryPiece> poetryPieces;

    protected override void OnStart()
    {
        currentPiece = 0;
        elapsed = 100000; //random big enough number
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        var poetryPiece = poetryPieces[currentPiece];
        elapsed += Time.deltaTime;

        if (elapsed > poetryPiece.timeForPiece)
        {
            elapsed = 0;
            Debug.Log(poetryPiece.piece);
            ++currentPiece;
        }


        if (currentPiece >= poetryPieces.Count)
        {
            return State.Success;
        }


        return State.Running;
    }
}
