using UnityEngine;
using UnityEngine.Events;

public class P_Controller : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;
    [SerializeField] private float groundCheckDistant = 0.5f;
    
    
    private float moveInput;
    

    private bool isOnGround;
    private bool isCanMove;
    private bool IsCanUseSkill;

    [SerializeField] private UnityEvent OnMoveEvent;
    [SerializeField] private UnityEvent<bool> OnJumpEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnLandEvent; // isOnGround
    [SerializeField] private UnityEvent<bool> OnBasicAttackEvent; // isCanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillOneEvent; // isCanUseSkill
    [SerializeField] private UnityEvent<bool> OnSkillTwoEvent; // isCanUseSkill

    private void Update()
    {
        if (isCanMove)
        { 
            

        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == Global.GroundLayerIndex))
            Helper_GroundCheck(collision);
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        if ((collision.gameObject.layer == Global.GroundLayerIndex))
            Helper_GroundCheck(collision);
    }

    private void Helper_GroundCheck(Collision2D collision)
    {
       RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistant, Global.GroundLayer);
        if (hit.collider != null)
        {
            isOnGround = true;
            OnLandEvent?.Invoke(isOnGround);
        }
        else
            isOnGround = false;
    }
}
