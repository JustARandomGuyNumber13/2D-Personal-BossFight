using System.Collections;
using UnityEngine;

public class Buff : MonoBehaviour
{
    public int StackCount {  get; private set; }
    public BuffType Type { get; private set; }


    public void P_AddStack(int amount)
    { StackCount += amount;}
    public void P_RemoveStack(int amount)
    { StackCount -= amount; }
    public void P_AddTemporaryStack(int amount, float duration)
    { 
        StartCoroutine(TemporaryStackCoroutine(amount, duration));
    }


    private IEnumerator TemporaryStackCoroutine(int amount, float duration)
    {
        StackCount += amount;
        yield return new WaitForSeconds(duration);
        StackCount -= amount;
    }
    public enum BuffType
    { 
        Poison,
        Fire,
        LifeSteal,
        Health,
        HealthRegen,
        Defense
    }
}
