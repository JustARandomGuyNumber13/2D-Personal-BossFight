using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class P_Stat : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;
    [SerializeField] private Vector3 groundCheckBoxSize;
    [SerializeField] private Vector3 groundCheckBoxOffset;
    [SerializeField] private Color groundCheckBoxColor = Color.blue;
    
    public bool OnGround { get; set; }
    public bool CanMove { get; private set; } = true;
    public bool CanUseSkill { get; private set; } = true;

    private float lifeStealPercentage;
    private float moveSpeedPercentage;
    private float damageAmountPercentage;
    private float jumpForcePercentage;
    
    public float MoveSpeed => charStat.MoveSpeed + (charStat.MoveSpeed * moveSpeedPercentage / 100);
    public float JumpForce =>  charStat.JumpForce + (charStat.JumpForce * jumpForcePercentage / 100);
    public UnityEvent OnLandEvent;

    private Rigidbody2D rb;
    private Health_Handler health;
    private float orgGravity;
    
    /* Monobehavior type */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        health = GetComponent<Health_Handler>();
    }
    private void Start()
    {
        orgGravity = rb.gravityScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == Global.GroundLayerIndex))
            Helper_GroundCheck(collision);
    }


    /* Helper type */
    private void Helper_GroundCheck(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + groundCheckBoxOffset, groundCheckBoxSize, 0, Vector2.zero, 0, Global.GroundLayer);

        if (hit.collider != null)
        {
            OnGround = true;
            OnLandEvent?.Invoke();
        }
    }


    /* Control type */
    public void Public_DealDamage(float amount, Health_Handler target)
    {
        float dmgAmount = amount + (amount * damageAmountPercentage / 100);
        target.Public_DecreaseHealth(dmgAmount); 
        if(lifeStealPercentage != 0) health.Public_IncreaseHealth(dmgAmount * lifeStealPercentage / 100);
    }
    public void Public_StopMoveOnGround()
    { if (OnGround) rb.linearVelocityX = 0; }
    public void Public_AddForce(Vector2 force, ForceMode2D forceMode)
    { rb.AddForce(force, forceMode); }
    public void Public_ResetGravity()
    { rb.gravityScale = orgGravity; }


    /* Buff type */
    public void Public_UseBuff(BuffType type, float percentage, float duration)
    { StartCoroutine(BuffCoroutine(type, percentage, duration)); }
    private IEnumerator BuffCoroutine(BuffType type, float percentage, float duration)
    {
        switch (type)
        {
            case BuffType.DamageAmount:
                damageAmountPercentage += percentage;
                break;
            case BuffType.JumpForce:
                jumpForcePercentage += percentage;
                break;
            case BuffType.MoveSpeed:
                moveSpeedPercentage += percentage; 
                break;
            case BuffType.LifeSteal:
                lifeStealPercentage += percentage;
                break;
        }

        if(duration != 0)
            yield return new WaitForSeconds(duration);

        switch (type)
        {
            case BuffType.DamageAmount:
                damageAmountPercentage -= percentage;
                break;
            case BuffType.JumpForce:
                jumpForcePercentage -= percentage;
                break;
            case BuffType.MoveSpeed:
                moveSpeedPercentage -= percentage;
                break;
            case BuffType.LifeSteal:
                lifeStealPercentage -= percentage;
                break;
        }
    }


    /* Optional type */
    public void Print(string text)
    { Debug.Log(text, gameObject); }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = groundCheckBoxColor;
        Gizmos.DrawWireCube(transform.position + groundCheckBoxOffset, groundCheckBoxSize);
    }

    public enum BuffType { MoveSpeed, DamageAmount, JumpForce, LifeSteal }
}
