using UnityEngine;

public class Effect
{
    [HideInInspector]
    public int strength;
    
    public virtual void ApplyEffect(Characteristics target, Characteristics source) {
        throw new System.NotImplementedException();
    }
}