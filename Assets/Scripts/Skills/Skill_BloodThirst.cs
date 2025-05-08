using UnityEngine;
using UnityEngine.Events;

public class Skill_BloodThirst : Skill
{
    [SerializeField] private UnityEvent<float> LifeStealEvent; // dmgAmount

    public void Public_LifeSteal(float dmgAmount)
    {
        if (pStat.LifeSteal)
            LifeStealEvent?.Invoke(dmgAmount * 0.2f);   // Health 20% of dmg dealt
    }
}
