using UnityEngine;
using UnityEngine.Events;

public class Skill_BloodThirst : Skill
{
    [Header("Skill Exclusive:")]
    [SerializeField] private Health_Handler pHealth;

    protected override void OnSkillTrigger()
    {

    }
}
