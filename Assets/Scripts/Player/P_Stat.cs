using UnityEngine;

public class P_Stat : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;

    public bool OnGround { get; set; }
    public bool CanMove { get; private set; } = true;
    
    public bool CanUseSkill { get; private set; } = true;
    public bool LifeSteal { get; private set; }

    public float MoveSpeed { get; private set; }
    public float JumpForce { get; private set; }
    public float Gravity { get; private set; }

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        MoveSpeed = charStat.MoveSpeed;
        JumpForce = charStat.JumpForce;
        Gravity = GetComponent<Rigidbody2D>().gravityScale;
    }

    public void Public_SetCanMove(bool value) { CanMove = value; }
    public void Public_SetCanUseSkill(bool value) { CanUseSkill = value; }
    public void Public_SetLifeSteal(bool value) { LifeSteal = value; }
}
