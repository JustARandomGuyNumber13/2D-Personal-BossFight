using UnityEngine;
using UnityEngine.Events;

public class P_Stat : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;
    [SerializeField] private Vector3 groundCheckBoxSize;
    [SerializeField] private Vector3 groundCheckBoxOffset;
    [SerializeField] private Color groundCheckBoxColor = Color.blue;
    [SerializeField] private UnityEvent<bool> OnLandEvent; // OnGround

    public bool OnGround { get; set; }
    public bool CanMove { get; private set; } = true;
    
    public bool CanUseSkill { get; private set; } = true;
    public bool LifeSteal { get; private set; }

    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    private float gravity;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        MoveSpeed = charStat.MoveSpeed;
        JumpForce = charStat.JumpForce;
        gravity = rb.gravityScale;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == Global.GroundLayerIndex))
            Helper_GroundCheck(collision);
    }

    /* Helper methods */
    private void Helper_GroundCheck(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + groundCheckBoxOffset, groundCheckBoxSize, 0, Vector2.zero, 0, Global.GroundLayer);

        if (hit.collider != null)
        {
            OnGround = true;
            OnLandEvent?.Invoke(OnGround);
        }
    }

    public void Public_SetCanMove(bool value) 
    { CanMove = value; }
    public void Public_SetCanUseSkill(bool value)
    { CanUseSkill = value; }
    public void Public_SetLifeSteal(bool value) 
    { LifeSteal = value; }
    public void Public_StopMoveOnGround()
    { if (OnGround) rb.linearVelocityX = 0; }
    public void Public_StopMove()
    { rb.linearVelocityX = 0; }
    public void Public_AddForce(Vector2 force, ForceMode2D forceMode)
    { rb.AddForce(force, forceMode); }
    public void Public_SetGravity(float value)
    { rb.gravityScale = value; }
    public void Public_ResetGravity()
    { rb.gravityScale = gravity; }
    public void Print(string text)
    { Debug.Log(text,gameObject); }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = groundCheckBoxColor;
        Gizmos.DrawWireCube(transform.position + groundCheckBoxOffset, groundCheckBoxSize);
    }
}
