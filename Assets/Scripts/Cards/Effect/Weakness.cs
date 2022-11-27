using UnityEngine;
public class Weakness : Effect
{
    public override void ApplyEffect(Characteristics target)
    {
        target.status.weakness += strength;
        Debug.Log("Apply Weakness on" + target.name);
    }
}
