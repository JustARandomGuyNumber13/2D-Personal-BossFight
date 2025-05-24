using UnityEngine;

public class Skill_SpawnObject : Skill
{
    [SerializeField] Projectile projectile;

    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        projectile.P_Launch(pStat.gameObject, transform);
    }
}
