using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Skill : MonoBehaviour
{
    [Header("Parent components:")]
    protected P_Stat pStat;
    [SerializeField] protected SO_SkillStat skillStat;
    [SerializeField] protected SkillType skillType = SkillType.Active;

    [SerializeField] private UnityEvent OnSkillDelayEvent;
    [SerializeField] private UnityEvent OnSkillTriggerEvent;
    [SerializeField] private UnityEvent OnSkillEndEvent;

    protected SkillState skillState = SkillState.Ready;


    protected virtual void Awake() { }
    protected virtual void Start() { }

    protected virtual IEnumerator SkillCoroutine()
    {
        OnSkillDelay();
        if(skillStat.SkillDelay > 0)
            yield return new WaitForSeconds(skillStat.SkillDelay);

        OnSkillTrigger();
        if(skillStat.SkillDuration > 0)
            yield return new WaitForSeconds(skillStat.SkillDuration);
        
        
        if(skillType == SkillType.Independent)
            yield return new WaitUntil(() => skillState == SkillState.CoolDown);
        else
            OnSkillEnd();

        if(skillStat.SkillCD > 0)
            yield return new WaitForSeconds(skillStat.SkillCD);

        skillState = SkillState.Ready;
        this.enabled = false;
    }
    protected virtual IEnumerator SkillCoolDownCoroutine()
    {
        OnSkillEnd();
        if (skillStat.SkillCD > 0)
            yield return new WaitForSeconds(skillStat.SkillCD);

        this.enabled = false;
    }


    /* Public handlers */
    public virtual void Public_ActivateSkill(P_Stat pStat) 
    {
        if ((skillState != SkillState.Ready)) return;

        this.pStat = pStat;
        this.enabled = true;
        StartCoroutine(SkillCoroutine());
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
        skillState = SkillState.Delay;
        OnSkillDelayEvent?.Invoke();
    }
    protected virtual void OnSkillTrigger() 
    {
        skillState = SkillState.Activating;
        OnSkillTriggerEvent?.Invoke();
    }
    protected virtual void OnSkillEnd() 
    {
        skillState = SkillState.CoolDown;
        OnSkillEndEvent?.Invoke();
    }
    public enum SkillType { Active, Passive, Independent}   // Indepent: Active & independent from skill duration
    protected enum SkillState{ Ready, Delay, Activating, CoolDown}
}
