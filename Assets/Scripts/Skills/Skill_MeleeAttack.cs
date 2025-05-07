using UnityEngine;

public class Skill_MeleeAttack : Skill
{
    private Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Global.EnemyLayerIndex)
            print("Hit enemy");
    }

    protected override void OnSkillTrigger()
    { 
        col.enabled = true;
    }
    protected override void OnSkillEnd()
    {
        base.OnSkillEnd();
        col.enabled = false;
    }
}
