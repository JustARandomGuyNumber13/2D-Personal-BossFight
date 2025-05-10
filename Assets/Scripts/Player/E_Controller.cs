using System.Collections;
using UnityEngine;

public class E_Controller : MonoBehaviour
{
    public enum EnemyState
    { 
        Idle,
        Waiting,
        Walking,
        CoolDown,
        Attacking,
         Die
    }

    [SerializeField] private P_Stat eStat;
    [SerializeField] private Transform pTransform;

    public EnemyState enemyState = EnemyState.Idle;
    [SerializeField] private E_Skill_Handler basicAttack, skillOne, skillTwo;
    private E_Skill_Handler curSkill;
    private Rigidbody2D rb;

    private int basicAtkCount;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        switch (enemyState)
        {
            case EnemyState.Walking:
                WalkToTarget();
                break;
                
            case EnemyState.Attacking:
                UseSkill();
                break;
        }
        
    }

    public void Public_Start()
    {
        ChooseSkill();
        enemyState = EnemyState.Walking;
    }
    public void Public_ResetState()
    {
        StartCoroutine(ResetStateCoroutine());
    }
    private IEnumerator ResetStateCoroutine()
    {
        enemyState = EnemyState.CoolDown;
        yield return new WaitForSeconds(curSkill.SkillCD);
        ChooseSkill();
        enemyState = EnemyState.Walking;
    }

    /* Skill handler */
    private void ChooseSkill()
    {
        if (basicAtkCount != 0)
        {
            curSkill = basicAttack;
            basicAtkCount--;
        }
        else
        {
            curSkill = Random.Range(1, 3) == 1 ? skillOne : skillTwo;
            basicAtkCount = Random.Range(1, 5);
        }
    }
    private void UseSkill()
    {
        curSkill.Public_ActivateSkill();
        enemyState = EnemyState.Waiting;
    }

    /* Movement handler */
    [SerializeField] float offSet;
    private void WalkToTarget()
    {
        if (eStat.CanMove)
        {
            LookAtTarget();
            rb.linearVelocityX = eStat.MoveSpeed * transform.lossyScale.x;
            if (Mathf.Abs(transform.position.x - pTransform.position.x) <= curSkill.SkillRequireRange)
            {
                enemyState = EnemyState.Attacking;
                rb.linearVelocityX = 0;
            }
        }
    }
    private void LookAtTarget()
    {
        if (transform.position.x < pTransform.position.x)
            transform.localScale = Vector3.one;
        else
            transform.localScale = new Vector3(-1, 1, 1);
    }
}

