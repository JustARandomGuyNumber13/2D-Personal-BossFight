using System.Collections;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class E_Controller : MonoBehaviour
{
    [SerializeField] private P_Stat eStat;
    [SerializeField] private Transform target;
    

    [SerializeField] private E_Skill_Handler basicAttack, skillOne, skillTwo;
    private Rigidbody2D rb;
    private Animator anim;
    private Transform _transform;

    float distance;
    private int basicAtkCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        _transform = rb.transform;
    }
    
    public bool P_UseSkill(int skillIndex)
    {
        bool result = false;
        distance = Vector3.Distance(_transform.position, target.position);
        switch (skillIndex)
        {
            case 0:
                if(distance <= basicAttack.SkillRequireRange)
                    if (basicAttack.Public_ActivateSkill()) result =  true;
                break;
            case 1:
                if (distance <= skillOne.SkillRequireRange)
                    if (skillOne.Public_ActivateSkill()) result = true;
                break;
            case 2:
                if (distance <= skillTwo.SkillRequireRange)
                    if (skillTwo.Public_ActivateSkill()) result = true;
                break;
        }

        if (result)
        {
            rb.linearVelocityX = 0;
            anim.SetFloat("moveSpeed", 0);
        }
        return result;
    }
    public E_Skill_Handler P_GetSkill(int skillIndex)
    {
        switch (skillIndex)
        {
            case 0:
                return basicAttack;
            case 1:
                return skillOne;
            case 2:
                return skillTwo;
        }
        return null;
    }

    /* Movement handler */
    [SerializeField] float offSet;
    public void P_WalkToTarget()
    {
        LookAtTarget();
        distance = Vector3.Distance(_transform.position, target.position);
        if (eStat.CanMove && Mathf.Abs(distance) > offSet)
        {
            rb.linearVelocityX = eStat.MoveSpeed * transform.lossyScale.x;
            anim.SetFloat("moveSpeed", 0.2f);
        }
        else
        {
            anim.SetFloat("moveSpeed", 0);
            rb.linearVelocityX = 0;
        }
    }

    bool lookRight;
    private void LookAtTarget()
    {
        lookRight = _transform.position.x < target.position.x;
        transform.localScale = new Vector3(lookRight ? 1 : -1, 1, 1);
    }
}

