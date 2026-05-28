using UnityEngine;

[NodeInfo("Move To Target", "Action/Movement/Move To Target")]
public class MoveNode : ActionNode
{
    public string transfromKey;
    public float speed;
    public float offset;

    private Transform targetTransform;
    protected override void OnStart()
    {
        targetTransform = this.blackboard[transfromKey] as Transform;
    }

    protected override void OnStop()
    {

    }

    protected override State OnUpdate()
    {
        if (!targetTransform)
            return State.Failure;

        //Fetch the current position
        Vector3 targetPosition = targetTransform.position;

        //Calculate the direction to the target 
        Vector3 direction = targetPosition - agent.gameObject.transform.position;
        direction = direction.normalized;

        this.agent.transform.position += direction * speed * Time.deltaTime;

        if ((this.agent.transform.position - targetPosition).magnitude < offset)
            return State.Success;

        return State.Running;
    }
}
