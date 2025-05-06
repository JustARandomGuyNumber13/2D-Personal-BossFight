using UnityEngine;

[CreateAssetMenu(fileName = "SO_CharStat", menuName = "Scriptable Objects/SO_CharStat")]
public class SO_CharStat : ScriptableObject
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float maxHealth;
    [SerializeField] private float defenseValue;

    public float MoveSpeed { get { return moveSpeed; }}
    public float JumpForce { get { return jumpForce; }}
    public float MaxHealth { get { return maxHealth; }}
    public float DefenseValue { get { return defenseValue; }}



}
