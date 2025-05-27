using UnityEngine;
using UnityEngine.Events;

public class Health_Handler : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;

    private float health;
    private P_Stat pStat;

    public UnityEvent<float> OnHealthDecreaseEvent; // health
    public UnityEvent<float> OnHealthIncreaseEvent; // health
    public UnityEvent OnDieEvent;

    private void Start()
    {
        health = charStat.MaxHealth;
        pStat = GetComponent<P_Stat>();
    }
    public float P_GetMaxHealth()
    {
        return charStat.MaxHealth;
    }
    public void Public_DecreaseHealth(float amount)
    {
        float dmgAmount = amount - charStat.DefenseValue;

        if (pStat.Defending)
            dmgAmount *= 0.5f;

        if (dmgAmount < 0) dmgAmount = 0;

        health -= dmgAmount;    
        OnHealthDecreaseEvent?.Invoke(health);

        if (health <= 0)
            OnDieEvent?.Invoke();
    }
    public void Public_DecreaseHealthIgnoreDefense(float amount)
    {
        health -= pStat.Defending ? amount * 0.5f : amount;
        OnHealthDecreaseEvent?.Invoke(health);

        if (health <= 0)
            OnDieEvent?.Invoke();
    }
    public void Public_IncreaseHealth(float amount)
    { 
        health += amount;
        if(health > charStat.MaxHealth) health = charStat.MaxHealth;
        OnHealthIncreaseEvent?.Invoke(health);
    }
}
