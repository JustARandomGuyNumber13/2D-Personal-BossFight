using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class P_Controller : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;
    
    [SerializeField] private float groundCheckDistance = 0.6f;
    
    private int moveInput;

    private readonly int moveAnimHash = Animator.StringToHash("moveInput");
    
    private bool isOnGround;
    public bool IsCanMove = true;
    public bool IsCanUseSkill = true;

    private Rigidbody2D rb;
    private Animator anim; // Only for horizontal movement

    [SerializeField] private UnityEvent<bool> OnJumpEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnLandEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnBasicAttackEvent; // isCanUseSkill
    [SerializeField] private UnityEvent<P_Controller> OnSkillOneEvent; // this
    [SerializeField] private UnityEvent<P_Controller> OnSkillTwoEvent; // this


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


    /* Action handlers */
    private void Action_Move()
    {
        if (IsCanMove)
        {
            rb.linearVelocityX = moveInput * charStat.MoveSpeed;

            if (anim.GetInteger(moveAnimHash) != moveInput)
                anim.SetInteger(moveAnimHash, moveInput);
        }
    }

    private void Action_Jump()
    {
        if (!IsCanMove || !IsCanUseSkill || !isOnGround) return;

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * charStat.JumpForce, ForceMode2D.Impulse);
        isOnGround = false;
        OnJumpEvent?.Invoke(isOnGround);
    }


    /* Helper methods */
    private void Helper_GroundCheck(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, Global.GroundLayer);

        if (hit.collider != null)
        {
            isOnGround = true;
            OnLandEvent?.Invoke(isOnGround);
        }
    }


    /* Input handlers*/
    private void OnMove(InputValue value)
    { 
        moveInput = (int) Mathf.Ceil(value.Get<float>());
    }
    private void OnJump()
    {
        Action_Jump();
    }
    private void OnSkillOne()
    {
        OnSkillOneEvent?.Invoke(this);
    }
    private void OnSkillTwo()
    {
        OnSkillTwoEvent?.Invoke(this);
    }
}
