using UnityEngine;
using UnityEngine.Events;

public class Skill_Leap : Skill
{
    /*
    Skill set up:
    Connect to: Skill prefab (include all below)
    Collider2d: Trigger collider 
    Rigbidbody 2d: Yes
    Layer: Default
     */
    [Header("Skill Exclusive:")]
    [SerializeField] private P_Controller pControl;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private Vector2 dmgRectangleSize;
    [SerializeField] private Vector2 dmgRectangleOffset;
    [SerializeField] private float gravityDuringLaunch;
    [SerializeField] private float dmgAmount;
    [SerializeField] private string targetLayerName;
    [SerializeField] private UnityEvent<Collider2D, float> OnTargetsContactEvent;

    private Collider2D col;
    private Rigidbody2D rb;

    protected override void Awake()
    {
        base.Awake();
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawCube(transform.position + (Vector3)dmgRectangleOffset, dmgRectangleSize);
    //}

    protected override void OnSkillTrigger()
    {
        base.OnSkillTrigger();
        pStat.OnGround = false;
        skillState = SkillState.Waiting;
        pControl.Public_SetGravity(gravityDuringLaunch);
        pControl.Public_AddForce(launchForce * transform.lossyScale, ForceMode2D.Impulse);
        Invoke("EnableCol", 0.1f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Global.GroundLayerIndex && rb.linearVelocityY == 0)
        {
            RaycastHit2D[] hitList = 
                Physics2D.BoxCastAll(transform.position + (Vector3)dmgRectangleOffset, dmgRectangleSize, 0, Vector2.zero, LayerMask.GetMask(targetLayerName));
            if (hitList.Length == 0) return;

            foreach (RaycastHit2D hit in hitList)
            {
                Health_Handler colHealth;
                hit.collider.TryGetComponent<Health_Handler>(out colHealth);

                if (colHealth != null)
                {
                    colHealth.Public_DecreaseHealth(dmgAmount);
                    OnTargetsContactEvent?.Invoke(hit.collider, dmgAmount);
                }
            }
            
            OnSkillEnd();
            pControl.Public_ResetGravity();
            col.enabled = false;
        }
    }

    private void EnableCol()
    { 
        if(skillState == SkillState.Waiting)
            col.enabled = true;
    }
}
