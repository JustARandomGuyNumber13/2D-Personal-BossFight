using UnityEngine;

public class P_Animator : MonoBehaviour
{
    private Animator anim;

    private int jumpHash = Animator.StringToHash("jump");
    private int isOnGroundHash = Animator.StringToHash("isOnGround");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Public_Jump()
    { 
        anim.SetTrigger(jumpHash);
        anim.SetBool(isOnGroundHash, false);
    }
    public void Public_Land()
    {
        anim.SetBool(isOnGroundHash, true);
    }
}
