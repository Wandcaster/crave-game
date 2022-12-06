using UnityEngine;

public class DealDamage : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source)
    {
        int tempStr = efficiency;

        tempStr += source.status.strength;
        if (target.status.vulnerability != 0) tempStr = (int)(tempStr * StatusEffects.vulnerabilityEfficiency);
        
        if (source.status.weakness != 0) tempStr= (int)(tempStr* StatusEffects.weaknessEfficiency);

        //Debug.Log(tempStr);

        //damage distribution on shield/hp
        if (target.status.shield != 0)
        {
            if (target.status.shield > tempStr) { target.status.shield -= tempStr; tempStr = 0; }
            else
            {
                tempStr -= target.status.shield;
                target.status.shield = 0;
            }
        }
        target.hp -= tempStr;
        Debug.Log("DealDamage: " + tempStr);
    }
}
