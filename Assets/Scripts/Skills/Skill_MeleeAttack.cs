using UnityEngine;
using UnityEngine.Events;

public class Skill_MeleeAttack : Skill
{
    [SerializeField] private float dmgAmount;
    private Collider2D col;

    [SerializeField] private UnityEvent<float> OnHitEvent; // dmgAmount

    protected override void Awake()
    {
        col = GetComponent<Collider2D>();
    }
    protected override void Start()
    {
        base.Start();
        col.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Global.EnemyLayerIndex)
        {
            collision.GetComponent<Health_Handler>().Public_DecreaseHealth(dmgAmount);
            col.enabled = false;
            OnHitEvent?.Invoke(dmgAmount);
        }
    }


    public override void Public_DeactivateSkill()
    {
        base.Public_DeactivateSkill();
        col.enabled = false;
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
