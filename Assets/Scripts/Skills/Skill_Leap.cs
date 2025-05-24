using UnityEngine;
using UnityEngine.Events;

public class Skill_Leap : Skill
{
    [Header("Skill Exclusive:")]
    [SerializeField] private string targetLayerName;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private float dmgAmount;
    [SerializeField] private Vector2 dmgRectangleSize;
    [SerializeField] private Vector2 dmgRectangleOffset;
    [SerializeField] private Color gizmosColor = Color.red;

    [SerializeField] private UnityEvent<float> OnHitEvent;
    private Vector3 launchDir;

    protected override void Start()
    {
        pStat.OnLandEvent.AddListener(OnLand);
    }
    public override void Public_ActivateSkill(P_Stat pStat)
    {
        if (!pStat.OnGround) return;
        base.Public_ActivateSkill(pStat);
    }
    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        launchDir = launchForce;
        launchDir.x *= transform.lossyScale.x;
        pStat.Public_AddForce(launchDir, ForceMode2D.Impulse);
    }

    private void OnLand()
    {
        if (!this.enabled || State != SkillState.Activating) return;

        RaycastHit2D[] hitList = 
            Physics2D.BoxCastAll(transform.position + (Vector3)dmgRectangleOffset, dmgRectangleSize, 0, Vector2.zero, 0, LayerMask.GetMask(targetLayerName));
        
        if (hitList.Length != 0)
            foreach (RaycastHit2D hit in hitList)
            {
                Health_Handler colHealth;
                hit.collider.TryGetComponent<Health_Handler>(out colHealth);

                if (colHealth != null)
                {
                    pStat.Public_DealDamage(dmgAmount, colHealth);
                    OnHitEvent?.Invoke(dmgAmount);
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
