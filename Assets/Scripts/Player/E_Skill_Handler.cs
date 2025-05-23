using UnityEngine;

public class E_Skill_Handler : MonoBehaviour
{
    public SO_SkillStat skillStat;
    [SerializeField] private Skill skill;
    [SerializeField] private float skillRequireRange;
    [SerializeField] private float skillCD;
    [SerializeField] private Color gizmosColor = Color.white;

    public float SkillCD { get { return skillCD; } }
    public float SkillRequireRange { get { return skillRequireRange; } }
    private P_Stat pStat;


    private void Awake()
    {
        pStat = GetComponent<P_Stat>();
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.right * skillRequireRange);
    }

    public void Public_ActivateSkill()
    {
        skill.Public_ActivateSkill(pStat);
    }
}
