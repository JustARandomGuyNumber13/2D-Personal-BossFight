using UnityEngine;

public class Skill_Buff : Skill
{
    [Header("Skill Exclusive:")]
    [SerializeField] private float lifeStealPercentage;
    [SerializeField] private float additionalMoveSpeedPercentage;
    [SerializeField] private float additionalJumpForcePercentage;
    [SerializeField] private float additionalDamagePercentage;

    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();

        if (lifeStealPercentage != 0)
            pStat.Public_UseBuff(P_Stat.BuffType.LifeSteal, lifeStealPercentage, skillStat.SkillDuration);

        if (additionalMoveSpeedPercentage != 0)
            pStat.Public_UseBuff(P_Stat.BuffType.MoveSpeed, additionalMoveSpeedPercentage, skillStat.SkillDuration);

        if (additionalJumpForcePercentage != 0)
            pStat.Public_UseBuff(P_Stat.BuffType.JumpForce, additionalJumpForcePercentage, skillStat.SkillDuration);

        if (additionalDamagePercentage != 0)
            pStat.Public_UseBuff(P_Stat.BuffType.DamageAmount, additionalDamagePercentage, skillStat.SkillDuration);
    }
}
