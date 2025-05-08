using UnityEngine;
using UnityEngine.Events;

public class Skill_BloodThirst : Skill
{
    [SerializeField] private int healingPercentage;
    [SerializeField] private Health_Handler pHealth;

    protected override void OnSkillTrigger()
    {
        pStat.Public_SetCanMove(true);
        pStat.Public_SetCanUseSkill(true);
    }
    public void Public_LifeSteal(float dmgAmount)
    {
        if (pStat.LifeSteal)
            pHealth.Public_IncreaseHealth(dmgAmount * healingPercentage / 100);   // Health 20% of dmg dealt
    }
}
