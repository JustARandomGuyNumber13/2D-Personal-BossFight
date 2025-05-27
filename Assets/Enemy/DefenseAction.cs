using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Defense", story: "Check if any of [Agent] skill is available, while not [Cooldown]", category: "Action", id: "3193671fb6b8398717b7826b0fb0747e")]
public partial class DefenseAction : Action
{
    [SerializeReference] public BlackboardVariable<E_Controller> Agent;
    [SerializeReference] public BlackboardVariable<bool> Cooldown;
    protected override Status OnStart()
    {
        if (!Agent.Value.P_SkillsAvailable())
        {
            
            Agent.Value.P_LookAtTarget();
            return Status.Success;
        }
        return Status.Failure;
    }
}

