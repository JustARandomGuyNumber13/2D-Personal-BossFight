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
    public UnityEvent OnLandEvent; // OnGround

    public bool OnGround { get; set; }
    public bool CanMove { get; private set; } = true;
    public bool CanUseSkill { get; private set; } = true;

    private Buff lifeStealBuff;
    private float lifeStealPercentage;

    public Buff AdditionalMoveSpeedBuff;
    private float additionalMoveSpeedPercentage;
    public float MoveSpeed => charStat.MoveSpeed + (charStat.MoveSpeed * additionalMoveSpeedPercentage / 100);

    private float additionalJumpForce;
    public float JumpForce => additionalJumpForce + charStat.JumpForce;

    private float additionalDamage;
    private float additionalDamagePercentage;


    private Rigidbody2D rb;
    private Health_Handler health;
    private float orgGravity;
    

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

    /* Modifier type */
    public void Public_Add_MoveSpeedPercentage(float value)
    { additionalMoveSpeedPercentage += value; }
    public void Public_Add_JumpForce(float value)
    { additionalJumpForce += value; }
    public void Public_Add_AdditionalDamage(float value)
    { additionalDamage += value; }
    public void Public_Add_AdditionalDamagePercentage(float value)
    { additionalDamagePercentage += value; }


    /* Setter type */
    public void Public_ResetGravity()
    { rb.gravityScale = orgGravity; }


    /* Action type */
    public void Public_DealDamage(float amount, Health_Handler target)
    {
        float dmgAmount = (amount + additionalDamage) +
            (amount + additionalDamage) * additionalDamagePercentage / 100;
        target.Public_DecreaseHealth(dmgAmount); 
        if(lifeStealBuff.Active) health.Public_IncreaseHealth(dmgAmount * lifeStealPercentage / 100);
    }
    public void Public_StopMoveOnGround()
    { if (OnGround) rb.linearVelocityX = 0; }
    public void Public_AddForce(Vector2 force, ForceMode2D forceMode)
    { rb.AddForce(force, forceMode); }

    /* Buff type */
    public void Public_UseBuff_LifeSteal(float duration, float percentage)
    {
        lifeStealPercentage = percentage;
        lifeStealBuff.Public_UseBuff(duration);
    }
    public void Public_UseBuff_AddtionalMoveSpeed(float duration, float percentage)
    { 
        additionalMoveSpeedPercentage = percentage;
        AdditionalMoveSpeedBuff.Public_UseBuff(duration);
    }


    public void Print(string text)
    { Debug.Log(text, gameObject); }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = groundCheckBoxColor;
        Gizmos.DrawWireCube(transform.position + groundCheckBoxOffset, groundCheckBoxSize);
    }
}
public class Buff : MonoBehaviour
{ 
    public bool Active;
    private Coroutine buffCoroutine;

    public void Public_UseBuff(float duration)
    {
        if(buffCoroutine != null)
            StopCoroutine(buffCoroutine);

        buffCoroutine = StartCoroutine(BuffCoroutine(duration));
    }
    private IEnumerator BuffCoroutine(float duration)
    {
        Active = true;
        yield return new WaitForSeconds(duration);
        Active = false;
    }
}
