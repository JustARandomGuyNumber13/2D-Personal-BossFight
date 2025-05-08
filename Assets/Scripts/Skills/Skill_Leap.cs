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

    [SerializeField] private Rigidbody2D pRb;
    [SerializeField] private Vector2 launchForce;
    [SerializeField] private Vector2 dmgRectangleSize;
    [SerializeField] private Vector2 dmgRectangleOffset;
    [SerializeField] private float dmgAmount;
    [SerializeField] private string targetLayerName;
    [SerializeField] private UnityEvent<Collider2D, float> OnTargetsContactEvent;

    private Collider2D col;

    protected override void Awake()
    {
        col = GetComponent<Collider2D>();
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
        col.enabled = true;
        pRb.AddForce(launchForce * pRb.transform.localScale, ForceMode2D.Impulse);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == Global.GroundLayerIndex && pRb.linearVelocityY <= 0)
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
            col.enabled = false;
        }
    }
}
