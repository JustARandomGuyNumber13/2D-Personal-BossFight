using UnityEngine;
using UnityEngine.Events;

public class Health_Handler : MonoBehaviour
{
    [SerializeField] private SO_CharStat charStat;

    private float health;

    [SerializeField] private UnityEvent<float> OnHealthDecreaseEvent; // health
    [SerializeField] private UnityEvent<float> OnHealthIncreaseEvent; // health

    private void Start()
    {
        health = charStat.MaxHealth;
    }

    public void Public_DecreaseHealth(float amount)
    {
        float dmgAmount = charStat.DefenseValue - amount;
        if (dmgAmount < 0) dmgAmount = 0;

        health -= amount;    
        OnHealthDecreaseEvent?.Invoke(health);
    }
    public void Public_DecreaseHealthIgnoreDefense(float amount)
    {
        health -= amount;
        OnHealthDecreaseEvent?.Invoke(health);
    }
    public void Public_IncreaseHealth(float amount)
    { 
        health += amount;
        if(health > charStat.MaxHealth) health = charStat.MaxHealth;
        OnHealthIncreaseEvent?.Invoke(health);
    }

    
}
