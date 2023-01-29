using UI;
using UnityEngine;

public class DealDamage : Effect
{
    public override void ApplyEffect(Characteristics target, Characteristics source, int efficiency)
    {
        int tempStr = efficiency;

        tempStr += source.status.strength.Value;
        if (target.status.vulnerability.Value != 0) tempStr = (int)(tempStr * StatusEffects.vulnerabilityEfficiency);
        
        if (source.status.weakness.Value != 0) tempStr= (int)(tempStr* StatusEffects.weaknessEfficiency);

        //Debug.Log(tempStr);

        //damage distribution on shield/hp
        if (target.status.shield.Value != 0)
        {
            if (target.status.shield.Value > tempStr) { target.status.shield.Value -= tempStr; tempStr = 0; }
            else
            {
                tempStr -= target.status.shield.Value;
                target.status.shield.Value = 0;
            }
        }
        target.hp=target.hp - tempStr ;
        target.gameObject.SetActive(target.IsAlive());
        Debug.Log(target+" "+source+" "+tempStr);
    }
}
