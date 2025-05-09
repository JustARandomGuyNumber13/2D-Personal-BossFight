using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    [Header("Parent components:")]
    [SerializeField] protected P_Stat pStat;
    [SerializeField] protected SO_SkillStat skillStat;

    [SerializeField] private UnityEvent OnSkillDelayEvent;
    [SerializeField] private UnityEvent OnSkillTriggerEvent;
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
                    OnSkillTrigger();
                }
                break;
            case SkillState.Activating:
                if (skillTimer >= skillStat.SkillDuration)
                {
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

        OnSkillDelay();
        this.enabled = true;
    }
    public virtual void Public_DeactivateSkill()
    {
        if (skillState == SkillState.CoolDown || !this.enabled) return;

        OnSkillEndEvent?.Invoke();
        OnSkillEnd();
    }


    /* Phases handlers */
    protected virtual void OnSkillDelay() 
    {
        OnSkillDelayEvent?.Invoke();
        skillState = SkillState.Delay;
        skillTimer = 0;
    }
    protected virtual void OnSkillTrigger() 
    {
        skillState = SkillState.Activating;
        OnSkillTriggerEvent?.Invoke();
        skillTimer = 0;
    }
    protected virtual void OnSkillEnd() 
    {
        OnSkillEndEvent?.Invoke();
        skillState = SkillState.CoolDown;
        skillTimer = 0;
    }
}
