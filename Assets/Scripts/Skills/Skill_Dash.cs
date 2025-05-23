using System.Collections;
using UnityEngine;

public class Skill_Dash : Skill
{
    [Header("Skill components")]
    [SerializeField] Vector3 direction;
    [SerializeField] string dashLayer;

    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        StartCoroutine(DashCoroutine());
    }

    private IEnumerator DashCoroutine()
    {
        float duration = skillStat.SkillDuration;
        Vector3 dir = direction;
        dir.x *= transform.lossyScale.x;
        while (duration > 0)
        {
            pStat.Public_SetVelocity(dir);
            yield return new WaitForSeconds(0.1f);
            duration -= 0.1f;
        }
    }
}
