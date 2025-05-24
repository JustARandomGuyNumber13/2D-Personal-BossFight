using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class P_InputHandler : MonoBehaviour
{
    [SerializeField] private P_Stat pStat;
    
    private Rigidbody2D rb;
    private Animator anim;

    private PlayerInput input;

    private readonly int moveAnimHash = Animator.StringToHash("moveInput");
    private readonly int runAnimHash = Animator.StringToHash("run");

    private int moveInput;

    [SerializeField] private UnityEvent OnJumpEvent; // pStat.OnGround
    [SerializeField] private UnityEvent<P_Stat> OnBasicAttackEvent; // pStat.CanUseSkill
    [SerializeField] private UnityEvent<P_Stat> OnSkillOneEvent; // pStat.CanUseSkill
    [SerializeField] private UnityEvent<P_Stat> OnSkillTwoEvent; // pStat.CanUseSkill


    /* Monobehavior methods */
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        Action_Move();
        Helper_MoveAnimation();
    }

    /* Action handlers */
    private void Action_Move()
    {
        if (pStat.CanMove && input.inputIsActive)
        {
            rb.linearVelocityX = moveInput * pStat.MoveSpeed;

            if (moveInput != 0 && moveInput != transform.localScale.x)
                transform.localScale = new Vector3(moveInput, 1, 1);
        }
    }
    private void Helper_MoveAnimation()
    { 
        if(anim.GetInteger(moveAnimHash) != moveInput)
            anim.SetInteger(moveAnimHash, moveInput);
    }

    private void Action_Jump()
    {
        if (!pStat.CanMove || !pStat.CanUseSkill || !pStat.OnGround) return;

        rb.linearVelocityY = 0;
        rb.AddForce(Vector2.up * pStat.JumpForce, ForceMode2D.Impulse);
        pStat.OnGround = false;
        OnJumpEvent?.Invoke();
    }

    /* Input handlers*/
    private void OnMove(InputValue value)
    { 
        moveInput = (int) Mathf.Ceil(value.Get<float>());
    }
    private void OnSprint(InputValue value)
    {
        anim.SetBool(runAnimHash, (int)Mathf.Ceil(value.Get<float>()) == 1);
    }
    private void OnJump(InputValue value)
    {
        if(Mathf.Ceil(value.Get<float>()) == 1)
            Action_Jump();
    }
    private void OnBasicAttack()
    {
        if (pStat.CanUseSkill) OnBasicAttackEvent?.Invoke(pStat);
    }
    private void OnSkillOne()
    {
        if (pStat.CanUseSkill) OnSkillOneEvent?.Invoke(pStat);
    }
    private void OnSkillTwo()
    {
        if (pStat.CanUseSkill) OnSkillTwoEvent?.Invoke(pStat);
    }
}
