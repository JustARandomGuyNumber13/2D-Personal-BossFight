using UnityEngine;
using UnityEngine.Events;

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

    public bool LifeSteal { get; private set; }
    private float lifeStealPercentage;

    private float additionalMoveSpeed;
    public float MoveSpeed => additionalMoveSpeed + charStat.MoveSpeed;

    private float additionalJumpForce;
    public float JumpForce => additionalJumpForce + charStat.JumpForce;

    
    private Rigidbody2D rb;
    private Health_Handler health;
    private float orgGravity;
    private float additionalDamage;

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
    public void Public_Add_MoveSpeed(float value)
    { additionalMoveSpeed += value; }
    public void Public_Add_JumpForce(float value)
    { additionalJumpForce += value; }
    public void Public_Add_AdditionalDamage(float value)
    { additionalDamage += value; }


    /* Setter type */
    public void Public_Set_LifeStealPercentage(float value)
    { lifeStealPercentage = value; }
    public void Public_ResetGravity()
    { rb.gravityScale = orgGravity; }


    /* Action type */
    public void Public_DealDamage(float amount, Health_Handler target)
    {
        float dmgAmount = amount + additionalDamage;
        target.Public_DecreaseHealth(dmgAmount); 
        if(LifeSteal) health.Public_IncreaseHealth(dmgAmount * lifeStealPercentage / 100);
    }
    public void Public_StopMoveOnGround()
    { if (OnGround) rb.linearVelocityX = 0; }
    public void Public_AddForce(Vector2 force, ForceMode2D forceMode)
    { rb.AddForce(force, forceMode); }


    public void Print(string text)
    { Debug.Log(text, gameObject); }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = groundCheckBoxColor;
        Gizmos.DrawWireCube(transform.position + groundCheckBoxOffset, groundCheckBoxSize);
    }
}
