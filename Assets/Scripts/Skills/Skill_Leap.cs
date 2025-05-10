using UnityEngine;
using UnityEngine.Events;

public class Skill_Leap : Skill
{

    [Header("Skill Exclusive:")]
    [SerializeField] private string targetLayerName;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private Vector2 dmgRectangleSize;
    [SerializeField] private Vector2 dmgRectangleOffset;
    [SerializeField] private float gravityDuringLaunch;
    [SerializeField] private float dmgAmount;
    [SerializeField] private Color gizmosColor = Color.red;

    [SerializeField] private UnityEvent<float> OnTargetsContactEvent;
 

    public override void Public_ActivateSkill(bool isCanUseSkill)
    {
        if (!pStat.OnGround) return;
        base.Public_ActivateSkill(isCanUseSkill);
    }
    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        pStat.OnGround = false;
        skillState = SkillState.Waiting;
        pStat.Public_SetGravity(gravityDuringLaunch);
        pStat.Public_AddForce(launchForce * transform.lossyScale, ForceMode2D.Impulse);
    }
    public void Public_OnLand()
    {
        if (!this.enabled || skillState != SkillState.Waiting) return;

        RaycastHit2D[] hitList = 
            Physics2D.BoxCastAll(transform.position + (Vector3)dmgRectangleOffset, dmgRectangleSize, 0, Vector2.zero, 0, LayerMask.GetMask(targetLayerName));
        
        if (hitList.Length != 0)
            foreach (RaycastHit2D hit in hitList)
            {
                Health_Handler colHealth;
                hit.collider.TryGetComponent<Health_Handler>(out colHealth);

                if (colHealth != null)
                {
                    colHealth.Public_DecreaseHealth(dmgAmount);
                    OnTargetsContactEvent?.Invoke(dmgAmount);
                }
            }

        pStat.Public_ResetGravity();
        OnSkillEnd();
    }


    Vector3 gizPos;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        gizPos = (Vector3)dmgRectangleOffset;
        gizPos.x *= transform.lossyScale.x;
        Gizmos.DrawWireCube(transform.position + gizPos, dmgRectangleSize);
    }
}
