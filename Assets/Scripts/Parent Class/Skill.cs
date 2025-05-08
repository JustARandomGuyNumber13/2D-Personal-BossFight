using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    [SerializeField] protected P_Stat pStat;
    [SerializeField] protected SO_SkillStat skillStat;

    [SerializeField] private UnityEvent OnSkillDelayEvent;
    [SerializeField] private UnityEvent OnSkillTriggerEvent;
    [SerializeField] private UnityEvent OnSkillCancelEvent;
    [SerializeField] private UnityEvent OnSkillEndEvent;

    protected virtual void Awake() { }
    protected virtual void Start() { }
    protected enum SkillState
    {
        Ready,
        Delay,
        Activating,
        Waiting,
        CoolDown
    }

    protected SkillState skillState = SkillState.Ready;
    private float skillTimer;

    private void Update()
    {
        switch (skillState)
        { 
            case SkillState.Delay:
                if (skillTimer >= skillStat.SkillDelay)
                {
                    OnSkillTriggerEvent?.Invoke();
                    skillState = SkillState.Activating;
                    skillTimer = 0;
                    OnSkillTrigger();
                }
                break;
            case SkillState.Activating:
                if (skillTimer >= skillStat.SkillDuration)
                {
                    OnSkillEndEvent?.Invoke();
                    OnSkillEnd();
                }
                break;
            case SkillState.CoolDown:
                if (skillTimer >= skillStat.SkillCD)
                {
                    skillState = SkillState.Ready;
                    skillTimer = 0;
                    this.enabled = false;
                }
                break;
        }
        skillTimer += Time.deltaTime;
    }


    /* Public handlers */
    public virtual void Public_ActivateSkill(bool isCanUseSkill) 
    {
        if (!isCanUseSkill || (skillState != SkillState.Ready)) return;

        OnSkillDelayEvent?.Invoke(); 
        skillState = SkillState.Delay;
        skillTimer = 0;
        OnSkillDelay();
        this.enabled = true;
    }
    public virtual void Public_DeactivateSkill()
    {
        if (skillState == SkillState.CoolDown || !this.enabled) return;

        OnSkillCancelEvent?.Invoke();
        OnSkillEnd();
    }


    /* Phases handlers */
    protected virtual void OnSkillDelay() 
    {
        pStat.Public_SetCanMove(false);
        pStat.Public_SetCanUseSkill(false);
        pStat.Public_StopMoveOnGround();
    }
    protected virtual void OnSkillTrigger() 
    {
        pStat.Public_SetCanMove(false);
        pStat.Public_SetCanUseSkill(false);
        pStat.Public_StopMoveOnGround();
    }
    protected virtual void OnSkillEnd() 
    {
        pStat.Public_SetCanMove(true);
        pStat.Public_SetCanUseSkill(true);
        skillState = SkillState.CoolDown;
        skillTimer = 0;
    }
}
