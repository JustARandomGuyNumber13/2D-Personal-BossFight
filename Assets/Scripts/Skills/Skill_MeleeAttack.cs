using UnityEngine;
using UnityEngine.Events;

public class Skill_MeleeAttack : Skill
{
    [Header("Skill Exclusive:")]
    [SerializeField] private float dmgAmount;
    private Collider2D col;

    [SerializeField] private UnityEvent<float> OnHitEvent; // dmgAmount

    protected override void Awake()
    {
        base.Awake();
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


    protected override void OnSkillTrigger()
    { 
        base.OnSkillTrigger();
        col.enabled = true;
    }
    protected override void OnSkillEnd()
    {
        base.OnSkillEnd();
        col.enabled = false;
    }
}
