using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Chase", story: "[Controller] move to target. Is walk: [Walk]", category: "Action", id: "cb01d9e9897a037faf505c2283d7149e")]
public partial class ChaseAction : Action
{
    [SerializeReference] public BlackboardVariable<E_Controller> Controller;
    [SerializeReference] public BlackboardVariable<bool> Walk;
    private Animator anim;
    protected override Status OnStart()
    {
        Controller.Value.P_MoveToTarget(Walk);
        return Status.Success;
    }

    protected override Status OnUpdate()
    {

        return Status.Success;
    }

    protected override void OnEnd()
    {
        
    }
}

