using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Threading;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack ", story: "If [Target] is in [Skill] range, use skill. Check [CD] and [Defense]", category: "Action", id: "495eddf04e1fc68674765442a79287b6")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<E_Controller> Target;
    [SerializeReference] public BlackboardVariable<int> Skill;
    [SerializeReference] public BlackboardVariable<bool> CD;
    [SerializeReference] public BlackboardVariable<bool> Defense;
    float timer;
    bool active;

    protected override Status OnStart()
    {
        if (!active && !CD)
        {
            bool success = Target.Value.P_UseSkill(Skill);
            if (success)
            {
                Target.Value.P_LookAtTarget();
                timer = Target.Value.P_GetSkill(Skill).SkillCD;
                active = true;
                CD.Value = true;
                Defense.Value = false;
                return Status.Success;
            }
        }

        if (!active)
        {
            return Status.Failure;
        }
        if (active && timer <= 0.0f)
        {
            CD.Value = false;
            active = false;
            return Status.Success;
        }

        return Status.Running;
    }


    protected override Status OnUpdate()
    {
        timer -= Time.deltaTime;
        if (active && timer <= 0)
        {
            CD.Value = false;
            active = false;
            return Status.Success;
        }

        return Status.Running;
    }
}

