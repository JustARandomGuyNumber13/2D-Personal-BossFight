using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Agent] chase [Target] at [MoveSpeed]", category: "Action", id: "cb01d9e9897a037faf505c2283d7149e")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<float> MoveSpeed;
    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        Vector3 direction = Vector3.Normalize(Target.Value.transform.position - Agent.Value.transform.position);
        direction.y = 0;
        direction.z = 0;
        Agent.Value.localScale = new Vector3(direction.x > 0 ? 1 : -1, 1, 1);
        direction *= MoveSpeed * Time.deltaTime;

        Agent.Value.Translate(direction * MoveSpeed * Time.deltaTime);
        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }
}

