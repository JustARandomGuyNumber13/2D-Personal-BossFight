using UnityEngine;
using UnityEngine.Events;

public class Skill_MeleeAttack : Skill
{
    [Header("Skill Exclusive:")]
    [SerializeField] private string targetLayerName;
    [SerializeField] private float dmgAmount;
    [SerializeField] private Vector3 atkBoxSize;
    [SerializeField] private Vector3 atkBoxOffset;
    [SerializeField] private Color gizmosColor = Color.red;

    [SerializeField] private UnityEvent<float> OnHitEvent; // dmgAmount

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawWireCube(transform.position + atkBoxOffset, atkBoxSize);
    }

    override protected void OnSkillTrigger()
    {
        base.OnSkillTrigger();

        RaycastHit2D[] hitList = 
            Physics2D.BoxCastAll(transform.position + atkBoxOffset, atkBoxSize, 0, Vector2.zero, 0, LayerMask.GetMask(targetLayerName));

        if (hitList.Length == 0) return;

        foreach (RaycastHit2D hit in hitList)
        {
            Health_Handler targetHealth;
            hit.collider.TryGetComponent<Health_Handler>(out targetHealth);

            if (targetHealth != null)
            {
                targetHealth.Public_DecreaseHealth(dmgAmount);
                OnHitEvent?.Invoke(dmgAmount);
            }
        }
    }

}
