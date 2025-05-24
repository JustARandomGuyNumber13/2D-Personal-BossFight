using UnityEngine;

public class E_Skill_Handler : MonoBehaviour
{
    private Skill skill;
    [SerializeField] private float skillRequireRange;
    [SerializeField] private float skillCD;
    [SerializeField] private Color gizmosColor = Color.white;

    public float SkillCD { get { return skillCD; } }
    public float SkillRequireRange { get { return skillRequireRange; } }
    private P_Stat pStat;


    private void Awake()
    {
        pStat = transform.parent.GetComponent<P_Stat>();
        skill = GetComponent<Skill>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * skillRequireRange);
    }

    public bool Public_ActivateSkill()
    {
        if (skill.State != Skill.SkillState.Ready) return false;
        skill.Public_ActivateSkill(pStat);
        return skill.State == Skill.SkillState.Activating;
    }
}
