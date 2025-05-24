using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using System.Threading;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Attack ", story: "If [Target] is in [Agent] [Skill] range, use skill then go to [Cooldown] mode", category: "Action", id: "495eddf04e1fc68674765442a79287b6")]
public partial class AttackAction : Action
{
    [SerializeReference] public BlackboardVariable<Transform> Target;
    [SerializeReference] public BlackboardVariable<Transform> Agent;
    [SerializeReference] public BlackboardVariable<E_Skill_Handler> Skill;
    [SerializeReference] public BlackboardVariable<bool> Cooldown;
    private float m_Timer = 0.0f;

    protected override Status OnStart()
    {
        float distance = Vector3.Distance(Target.Value.position, Agent.Value.position);
        if (distance <= Skill.Value.SkillRequireRange && Skill.Value.Public_ActivateSkill())
        {
            Cooldown.Value = true;
            m_Timer = Skill.Value.SkillCD;
        }
        else
            return Status.Failure;

        if (m_Timer <= 0.0f)
        {
            Debug.Log("Cooldown end");
            Cooldown.Value = false;
            return Status.Success;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        m_Timer -= Time.deltaTime;
        if (m_Timer <= 0)
        {
            return Status.Success;
        }
        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

