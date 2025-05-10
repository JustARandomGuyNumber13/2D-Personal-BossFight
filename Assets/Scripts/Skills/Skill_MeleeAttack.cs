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

    override protected void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        Vector3 hitPos = transform.position;
        hitPos.x += transform.lossyScale.x * atkBoxOffset.x;
        hitPos.y += atkBoxOffset.y;

        RaycastHit2D[] hitList = 
            Physics2D.BoxCastAll(hitPos, atkBoxSize, 0, Vector3.zero, 0, LayerMask.GetMask(targetLayerName));

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


    Vector3 gizPos;
    Vector3 gizBox;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        gizPos = atkBoxOffset;
        gizPos.x *= transform.lossyScale.x;
        gizBox = atkBoxSize;
        gizBox.x *= transform.lossyScale.x;
        Gizmos.DrawWireCube(transform.position + gizPos, gizBox);
    }
}
