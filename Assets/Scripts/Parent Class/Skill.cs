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
    protected virtual void Start()
    {
        this.enabled = false;
    }
    private enum SkillState
    {
        Ready,
        Delay,
        Activating,
        CoolDown
    }

    private SkillState skillState = SkillState.Ready;
    private float skillTimer;

    private void Update()
    {
        switch (skillState)
        { 
            case SkillState.Delay:
                if (skillTimer >= skillStat.SkillDelay)
                {
                    OnSkillTrigger();
                    OnSkillTriggerEvent?.Invoke();
                    skillState = SkillState.Activating;
                    skillTimer = 0;
                }
                break;
            case SkillState.Activating:
                if (skillTimer >= skillStat.SkillDuration)
                {
                    OnSkillEnd();
                    OnSkillEndEvent?.Invoke();
                    skillState = SkillState.CoolDown;
                    skillTimer = 0;
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
        OnSkillDelayEvent?.Invoke(); 
        skillState = SkillState.Delay;
        this.enabled = true;
    }
    public virtual void Public_DeactivateSkill()
    {
        if (skillState == SkillState.CoolDown || !this.enabled) return;

        OnSkillCancelEvent?.Invoke();
        OnSkillEnd();
        skillTimer = 0;
        skillState = SkillState.CoolDown;
    }


    /* Phases handlers */
    protected virtual void OnSkillDelay() { }
    protected virtual void OnSkillTrigger() { }
    protected virtual void OnSkillEnd() { }
}
