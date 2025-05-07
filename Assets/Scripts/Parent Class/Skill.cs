using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    [SerializeField] private SO_SkillStat skillStat;
    [SerializeField] private P_Controller p;

    [SerializeField] private UnityEvent OnSkillDelayEvent;
    [SerializeField] private UnityEvent OnSkillTriggerEvent;
    [SerializeField] private UnityEvent OnSkillEndEvent;

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
                if (skillTimer >= skillStat.SkillDuration)
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
    public void Public_ActivateSkill(P_Controller p) 
    {
        if (!p.IsCanUseSkill || (skillState != SkillState.Ready)) return;

        OnSkillDelay();
        OnSkillDelayEvent?.Invoke(); 
        skillState = SkillState.Delay;
        p.IsCanUseSkill = false;
        this.p = p;
        this.enabled = true;
    }
    public void Public_DeactivateSkill()
    {
        if (skillState == SkillState.CoolDown) return;

        OnSkillEnd();
        skillTimer = 0;
        skillState = SkillState.CoolDown;
    }


    /* Phases handlers */
    protected virtual void OnSkillDelay() 
    {
        p.IsCanMove = false;
        p.IsCanUseSkill = false;
    }
    protected virtual void OnSkillTrigger() { }
    protected virtual void OnSkillEnd() 
    {
        p.IsCanUseSkill = true;
        p.IsCanMove = true;
    }
}
