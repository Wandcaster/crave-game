using UnityEngine;
public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        target.status.weakness += efficiency;
        Debug.Log("Apply Weakness on" + target.characteristicName);
    }
}
