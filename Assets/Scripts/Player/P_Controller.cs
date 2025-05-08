using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class P_Controller : MonoBehaviour
{
    [SerializeField] private P_Stat stat;
    [SerializeField] private Vector3 groundCheckBoxSize;
    [SerializeField] private Vector3 groundCheckBoxOffset;
    
    private Rigidbody2D rb;
    private Animator anim; // Only for horizontal movement
    private readonly int moveAnimHash = Animator.StringToHash("moveInput");
    private int moveInput;

    [SerializeField] private UnityEvent<bool> OnJumpEvent; // stat.OnGround
    [SerializeField] private UnityEvent<bool> OnLandEvent; // stat.OnGround
    [SerializeField] private UnityEvent<bool> OnBasicAttackEvent; // stat.CanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillOneEvent; // stat.CanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillTwoEvent; // stat.CanUseSkill


    /* Monobehavior methods */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    private void Update()
    {
        Action_Move();
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == Global.GroundLayerIndex))
            Helper_GroundCheck(collision);
    }
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawCube(transform.position + groundCheckBoxOffset, groundCheckBoxSize);
    //}


    /* Action handlers */
    private void Action_Move()
    {
        if (stat.CanMove)
        {
            rb.linearVelocityX = moveInput * stat.MoveSpeed;

            if (moveInput != 0 && moveInput != transform.localScale.x)
                transform.localScale = new Vector3(moveInput, 1, 1);
            
            if (anim.GetInteger(moveAnimHash) != moveInput)
                anim.SetInteger(moveAnimHash, moveInput);
        }
    }
    private void Action_Jump()
    {
        if (!stat.CanMove || !stat.CanUseSkill || !stat.OnGround) return;

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * stat.JumpForce, ForceMode2D.Impulse);
        stat.OnGround = false;
        OnJumpEvent?.Invoke(stat.OnGround);
    }


    /* Helper methods */
    private void Helper_GroundCheck(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + groundCheckBoxOffset, groundCheckBoxSize, 0, Vector2.zero, 0, Global.GroundLayer);

        if (hit.collider != null)
        {
            stat.OnGround = true;
            OnLandEvent?.Invoke(stat.OnGround);
        }
    }


    /* Input handlers*/
    private void OnMove(InputValue value)
    { 
        moveInput = (int) Mathf.Ceil(value.Get<float>());
    }
    private void OnJump(InputValue value)
    {
        if(Mathf.Ceil(value.Get<float>()) == 1)
            Action_Jump();
    }
    private void OnBasicAttack()
    {
        OnBasicAttackEvent?.Invoke(stat.CanUseSkill);
    }
    private void OnSkillOne()
    {
        OnSkillOneEvent?.Invoke(stat.CanUseSkill);
    }
    private void OnSkillTwo()
    {
        OnSkillTwoEvent?.Invoke(stat.CanUseSkill);
    }
}
