using UnityEngine;
public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        target.status.weakness += strength;
        Debug.Log("Apply Weakness on" + target.characteristicName);
    }
}
