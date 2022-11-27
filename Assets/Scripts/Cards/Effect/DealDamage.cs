using UnityEngine;

public class DealDamage : Effect
{
    public override void ApplyEffect(Characteristics target)
    {
        if (target.status.vulnerability != 0) strength = (int)(strength * StatusEffects.vulnerabilityEfficiency);

        if (target.status.shield != 0)
        {
            if (target.status.shield > strength) { target.status.shield -= strength; }
            else
            {
                strength -= target.status.shield;
                target.status.shield = 0;
            }
        }
        target.hp -= strength;
        Debug.Log("DealDamage: " + strength);
    }
}
