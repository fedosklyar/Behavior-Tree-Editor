using UnityEngine;

public class TestNode : Node
{
    public int startCount = 0;
    public int stopCount = 0;
    public State returnState = State.Running;

    protected override void OnStart() => startCount++;
    protected override void OnStop() => stopCount++;
    protected override State OnUpdate() => returnState;
}
