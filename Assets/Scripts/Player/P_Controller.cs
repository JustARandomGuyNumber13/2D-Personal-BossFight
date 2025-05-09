using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class P_Controller : MonoBehaviour
{
    [SerializeField] private P_Stat pStat;
    [SerializeField] private Vector3 groundCheckBoxSize;
    [SerializeField] private Vector3 groundCheckBoxOffset;
    
    private Rigidbody2D rb;
    private Animator anim; // Only for horizontal movement
    private readonly int moveAnimHash = Animator.StringToHash("moveInput");
    private int moveInput;

    [SerializeField] private UnityEvent<bool> OnJumpEvent; // pStat.OnGround
    [SerializeField] private UnityEvent<bool> OnLandEvent; // pStat.OnGround
    [SerializeField] private UnityEvent<bool> OnBasicAttackEvent; // pStat.CanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillOneEvent; // pStat.CanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillTwoEvent; // pStat.CanUseSkill


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
        if (pStat.CanMove)
        {
            rb.linearVelocityX = moveInput * pStat.MoveSpeed;

            if (moveInput != 0 && moveInput != transform.localScale.x)
                transform.localScale = new Vector3(moveInput, 1, 1);
            
            if (anim.GetInteger(moveAnimHash) != moveInput)
                anim.SetInteger(moveAnimHash, moveInput);
        }
    }
    private void Action_Jump()
    {
        if (!pStat.CanMove || !pStat.CanUseSkill || !pStat.OnGround) return;

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * pStat.JumpForce, ForceMode2D.Impulse);
        pStat.OnGround = false;
        OnJumpEvent?.Invoke(pStat.OnGround);
    }


    /* Helper methods */
    private void Helper_GroundCheck(Collision2D collision)
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position + groundCheckBoxOffset, groundCheckBoxSize, 0, Vector2.zero, 0, Global.GroundLayer);

        if (hit.collider != null)
        {
            pStat.OnGround = true;
            OnLandEvent?.Invoke(pStat.OnGround);
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
        OnBasicAttackEvent?.Invoke(pStat.CanUseSkill);
    }
    private void OnSkillOne()
    {
        OnSkillOneEvent?.Invoke(pStat.CanUseSkill);
    }
    private void OnSkillTwo()
    {
        OnSkillTwoEvent?.Invoke(pStat.CanUseSkill);
    }
}
