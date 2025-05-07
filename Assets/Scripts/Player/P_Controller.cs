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
    private bool isCanMove = true;
    private bool isCanUseSkill = true;

    private Rigidbody2D rb;
    private Animator anim; // Only for horizontal movement

    [SerializeField] private UnityEvent<bool> OnJumpEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnLandEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnBasicAttackEvent; // isCanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillOneEvent; // isCanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillTwoEvent; // isCanUseSkill


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
        if (isCanMove)
        {
            rb.linearVelocityX = moveInput * charStat.MoveSpeed;

            if (moveInput != 0 && moveInput != transform.localScale.x)
                transform.localScale = new Vector3(moveInput, 1, 1);
            
            if (anim.GetInteger(moveAnimHash) != moveInput)
                anim.SetInteger(moveAnimHash, moveInput);
        }
    }
    private void Action_Jump()
    {
        if (!isCanMove || !isCanUseSkill || !isOnGround) return;

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
    private void OnJump(InputValue value)
    {
        if(Mathf.Ceil(value.Get<float>()) == 1)
            Action_Jump();
    }
    private void OnBasicAttack()
    {
        OnBasicAttackEvent?.Invoke(isCanUseSkill);
    }
    private void OnSkillOne()
    {
        OnSkillOneEvent?.Invoke(isCanUseSkill);
    }
    private void OnSkillTwo()
    {
        OnSkillTwoEvent?.Invoke(isCanUseSkill);
    }

    /* Public methods */
    public void Public_StopMove()
    { rb.linearVelocityX = 0; }
    public void Public_SetIsCanMove(bool value) { isCanMove = value; }
    public void Public_SetIsCanUseSkill(bool value) { isCanUseSkill = value; }
}
