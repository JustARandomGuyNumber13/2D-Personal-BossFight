using UnityEngine;

public class P_Stat : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;

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
}
