using UnityEngine;

public class P_Animator : MonoBehaviour
{
    private Animator anim;

    private int jumpHash = Animator.StringToHash("jump");
    private readonly int landAnimHash = Animator.StringToHash("land");
    private int useSkillHash = Animator.StringToHash("useSkill");
    private int skillIndexHash = Animator.StringToHash("skillIndex");

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void Public_Jump()
    { 
        anim.SetTrigger(jumpHash);
        anim.SetBool(landAnimHash, false);
    }
    public void Public_Land()
    {
        anim.SetBool(landAnimHash, true);
    }
    public void Public_UseSkill(int index)
    { 
        anim.SetInteger(skillIndexHash, index);
        anim.SetTrigger(useSkillHash);
    }
}
