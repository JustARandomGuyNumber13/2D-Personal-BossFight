using System.Collections;
using UnityEngine;

public class Skill_Dash : Skill
{
    [Header("Skill components")]
    [SerializeField] Vector3 direction;
    [SerializeField] string dashLayer;
    [SerializeField] float damage;

    Collider2D previousTarget;
    Health_Handler health;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (previousTarget == null || previousTarget != collision)
        {
            Health_Handler temp; collision.TryGetComponent<Health_Handler>(out temp);

            if (temp != null)
            {
                previousTarget = collision;
                health = temp;
            }
            else
                return;
        }
        health.Public_DecreaseHealth(damage);
    }

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
