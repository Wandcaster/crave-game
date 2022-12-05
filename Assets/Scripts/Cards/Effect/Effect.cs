using UnityEngine;

public class Effect
{
    [HideInInspector]
    public int efficiency;
    
    public virtual void ApplyEffect(Characteristics target, Characteristics source) {
        throw new System.NotImplementedException();
    }
}