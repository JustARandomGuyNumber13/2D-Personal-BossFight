using UnityEngine;

[CreateAssetMenu(fileName = "SO_SkillStat", menuName = "Scriptable Objects/SO_SkillStat")]
public class SO_SkillStat : ScriptableObject
{
    [SerializeField] private string skillName;
    [SerializeField] private Sprite skillImage;
    [SerializeField] private float skillDelay;
    [SerializeField] private float skillDuration;
    [SerializeField] private float skillCD;
    

    public string SkillName { get { return skillName; } }
    public Sprite SkillImage { get { return skillImage; } }
    public float SkillDelay {  get { return skillDelay; } }
    public float SkillDuration { get { return skillDuration; } }
    public float SkillCD { get { return skillCD; } }
    
}
